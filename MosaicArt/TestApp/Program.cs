using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MessagePack;
using MosaicArt.Core;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using static MosaicArt.Core.Utility;

namespace MosaicArt.TestApp
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // 分割数
            int divisionsX = 100;
            int divisionsY = 100;

            const string DirectoryPath = @"D:\Develop\Projects\MosaicArt\TestData\Resource";
            //var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\400x400.jpg";
            //var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon1000x1000.jpg";
            //var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\Twitter1000x1000.jpg";
            var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon1000x1000.jpg";

            Console.WriteLine("素材作成");
            {
                var files = Directory.GetFiles(DirectoryPath, "*.mp4");
                foreach (var file in files)
                {
                    var directory = Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file);
                    if (Directory.Exists(directory))
                    {
                        continue;// すでにあるならスキップ
                    }
                    MovieSlicer(file, 100, 32, 18);
                }
            }

            Console.WriteLine("素材分析");
            ImagesInfo imagesInfo = new ImagesInfo();
            var directories = Directory.GetDirectories(DirectoryPath);
            foreach (var d in directories)
            {
                bool newCreate = true;
                // 既存のImagesInfoを確認
                // 無いorVersionが古かったら作成する。
                var path = d + ImagesInfo.PathExtension;
                if (File.Exists(path))
                {
                    // ImageInfoがあればVersionを確認
                    var info = VersionInfo.Load(path);
                    if (info.Version == ImagesInfo.CurrentVersion)
                    {
                        var imagesInfo2 = ImagesInfo.Load(path);
                        imagesInfo.ImageInfos.AddRange(imagesInfo2.ImageInfos);
                        newCreate = false;
                    }
                }
                // 新規作成
                if (newCreate)
                {
                    var imagesInfo2 = new ImagesInfo(d);
                    imagesInfo.ImageInfos.AddRange(imagesInfo2.ImageInfos);
                    imagesInfo2.Save(path);
                }
            }
            if (imagesInfo.ImageInfos.Count <= 0)
            {
                Console.WriteLine("リソースがありません");
                return;
            }
            // 設計図
            Dictionary<System.Drawing.Point, ImageInfo?> bluePrint = new();
            int bluePrintWidth;
            int bluePrintHeight;
            // targetの分析、resourceと比較
            Console.WriteLine("分析");
            try
            {
                Bitmap bitmap = new Bitmap(targetPath);
                var width = bitmap.Width;
                var height = bitmap.Height;
                var w = bitmap.Width / divisionsX;
                var h = bitmap.Height / divisionsY;
                for (int y = 0; y < height; y += h)
                {
                    for (int x = 0; x < width; x += w)
                    {
                        var rect = new Rectangle(x, y, w, h);
                        var clippedBitmap = bitmap.Clip(rect);
                        var imageInfo = new ImageInfo(clippedBitmap);
                        // 最も近い画像を選ぶ
                        var nearImageInfo = imagesInfo.GetNear(imageInfo);
                        bluePrint.Add(new System.Drawing.Point(x, y), nearImageInfo);
                    }
                }
                bluePrintWidth = width;
                bluePrintHeight = height;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
            // モザイクアート生成
            Console.WriteLine("モザイクアート生成");
            {
                Bitmap bitmap = new Bitmap(targetPath);
                //Bitmap bitmap = new Bitmap(bluePrintWidth, bluePrintHeight);
                var w = bitmap.Width / divisionsX;
                var h = bitmap.Height / divisionsY;
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    //graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;// ぼやける
                    //graphics.InterpolationMode = InterpolationMode.Bicubic;// ぼやける
                    //graphics.InterpolationMode = InterpolationMode.Low;// ぼやける
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;// OK

                    //graphics.PixelOffsetMode = PixelOffsetMode.None;// 1/4しか描画されない
                    //graphics.PixelOffsetMode = PixelOffsetMode.HighSpeed;// 1/4しか描画されない
                    //graphics.PixelOffsetMode = PixelOffsetMode.Half;// OK
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;// OK

                    // アンチエイリアスしない 関係ない
                    //graphics.SmoothingMode = SmoothingMode.None;
                    //graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.SmoothingMode = SmoothingMode.HighQuality;

                    //graphics.Clear(Color.Pink);
                    foreach (var item in bluePrint)
                    {
                        var point = item.Key;
                        var imageInfo = item.Value;
                        if (imageInfo != null)
                        {
                            var clippedBitmap = new Bitmap(imageInfo.Path);
                            graphics.DrawImage(clippedBitmap, point.X, point.Y, w, h);
                        }
                    }
                }
                // 保存
                var destinationPath = Path.GetDirectoryName(targetPath);
                destinationPath += "/";
                destinationPath += Path.GetFileNameWithoutExtension(targetPath);
                destinationPath += "_MosaicArt.png";
                bitmap.Save(destinationPath, ImageFormat.Png);
            }
            Console.WriteLine("完了");
        }

        static void ImageSlicer(string imagePath, int divisionsX, int divisionsY)
        {
            var directoryPath = Path.GetDirectoryName(imagePath) + "/" + Path.GetFileNameWithoutExtension(imagePath);
            Directory.CreateDirectory(directoryPath);
            Bitmap bitmap = new Bitmap(imagePath);
            var width = bitmap.Width;
            var height = bitmap.Height;
            var w = bitmap.Width / divisionsX;
            var h = bitmap.Height / divisionsY;
            for (int y = 0; y < height; y += h)
            {
                for (int x = 0; x < width; x += w)
                {
                    var rect = new Rectangle(x, y, w, h);
                    var clippedBitmap = bitmap.Clip(rect);
                    clippedBitmap.Save($@"{directoryPath}/{x}_{y}.png", ImageFormat.Png);
                }
            }
        }

        static double CompareBitmap(Bitmap bitmap0, Bitmap bitmap1)
        {
            var width = bitmap0.Width;
            var height = bitmap0.Height;
            if (width != bitmap1.Width)
            {
                return -1;
            }
            if (height != bitmap1.Height)
            {
                return -1;
            }

            double result = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color0 = bitmap0.GetPixel(x, y);
                    var color1 = bitmap1.GetPixel(x, y);
                    result += Distance(color0, color1);
                }
            }
            return result;
        }
        /// <summary>
        /// 動画をフレームごとに画像に保存
        /// </summary>
        /// <param name="path">動画ファイルのパス</param>
        /// <param name="count">出力枚数</param>
        /// <param name="width">出力サイズの幅</param>
        /// <param name="height">出力サイズの高さ</param>
        static void MovieSlicer(string path, int count, int width, int height)
        {
            Console.WriteLine($"{path}, {count}, {width}, {height}");
            using (var capture = new VideoCapture(path))
            {
                var directory = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }
                var img = new Mat();
                var frameCount = capture.FrameCount - 1;// 実際に使えるのは1フレーム少ない
                if (count > frameCount)
                {
                    count = frameCount;
                }
                var interval = frameCount / count;
                for (int i = 0; i < frameCount; i += interval)
                {
                    capture.PosFrames = i;
                    capture.Read(img);
                    var bitmap = BitmapConverter.ToBitmap(img);
                    var resizeBitmap = new Bitmap(bitmap, width, height);
                    resizeBitmap.Save($@"{directory}/{i}.png", ImageFormat.Png);
                    Console.WriteLine($"PosFrames={i}");
                }
            }
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}