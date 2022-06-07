#define ENABLE_PARALLELS
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
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"開始 {startTime}");
            // 分割数
            int divisionsX = 100;
            int divisionsY = 100;

            const string DirectoryPath = @"D:\Develop\Projects\MosaicArt\TestData\Resource";
            //var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\マリン出航！_3500x2000.png";
            var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\Twitter400x400.jpg";
            Console.WriteLine($"{nameof(targetPath)}={targetPath}");

            Console.WriteLine("素材作成");
            {
                bool isRemake = false;
                var files = Directory.GetFiles(DirectoryPath, "*.mp4");
                foreach (var file in files)
                {
                    var directory = Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file);
                    if (Directory.Exists(directory) == false || isRemake)
                    {
                        MovieSlicer(file, 200);
                    }
                }
            }

            Console.WriteLine("素材分析");
            ImagesInfo imagesInfo = new ImagesInfo();
            var directories = Directory.GetDirectories(DirectoryPath);
            foreach (var d in directories)
            {
                bool isRemake = false;
                bool newCreate = true;
                // 既存のImagesInfoを確認
                // 無いorVersionが古かったら作成する。
                var path = d + ImagesInfo.PathExtension;
                if (File.Exists(path) && isRemake == false)
                {
                    // ImageInfoがあればVersionを確認
                    var info = VersionInfo.Load(path);
                    if (info.Version == ImagesInfo.CurrentVersion)
                    {
                        var imagesInfo2 = ImagesInfo.Load(path);
                        if (imagesInfo2.ExistsFile())
                        {
                            imagesInfo.ImageInfos.AddRange(imagesInfo2.ImageInfos);
                            newCreate = false;
                        }
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

            Console.WriteLine("素材整理");
            {
                imagesInfo.RemoveDuplicates();
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
                var w = width / divisionsX;
                var h = height / divisionsY;
                if (width % divisionsX != 0)
                {
                    Console.WriteLine($"{nameof(width)}が{divisionsX}の倍数ではありません。");
                    return;
                }
                if (height % divisionsY != 0)
                {
                    Console.WriteLine($"{nameof(height)}が{divisionsY}の倍数ではありません。");
                    return;
                }

                List<System.Drawing.Point> points = new();

                for (int y = 0; y < height; y += h)
                {
                    for (int x = 0; x < width; x += w)
                    {
                        points.Add(new System.Drawing.Point(x, y));
                    }
                }
                Shuffle(points);
                Console.WriteLine($"{nameof(points.Count)}={points.Count}");
#if ENABLE_PARALLELS
                Parallel.For(0, points.Count, i =>
                {
                    var point = points[i];
#else
                int i = 0;
                foreach (var point in points)
                {
#endif
                    var x = point.X;
                    var y = point.Y;
                    Console.WriteLine($"{i}:{x}x{y}");
                    var rect = new Rectangle(x, y, w, h);
                    Bitmap clippedBitmap;
                    lock (bitmap)
                    {
                        clippedBitmap = bitmap.Clip(rect);
                    }
                    var imageInfo = new ImageInfo(clippedBitmap);
                    // 最も近い画像を選ぶ
                    var nearImageInfo = imagesInfo.GetNear(imageInfo);
                    if (nearImageInfo != null)
                    {
                        lock (nearImageInfo)
                        {
                            nearImageInfo.IsReserved = true;
                        }
                    }
                    lock (bluePrint)
                    {
                        bluePrint.Add(new System.Drawing.Point(x, y), nearImageInfo);
                    }
#if ENABLE_PARALLELS
                });
#else
                     i++;
                }
#endif
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
                destinationPath += DateTime.Now.ToString("(yyyyMMdd_HHmmss)");
                destinationPath += ".png";
                bitmap.Save(destinationPath, ImageFormat.Png);
            }
            DateTime endTime = DateTime.Now;
            Console.WriteLine($"完了 {(endTime - startTime)}");
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
        /// <param name="interval">出力枚数</param>
        static void MovieSlicer(string path, int count)
        {
            Console.WriteLine($"{path}, {count}");
            using (var capture = new VideoCapture(path))
            {
                var directory = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }
                var img = new Mat();
                var frameCount = capture.FrameCount - 1;// 実際に使えるのは1フレーム少ない
                var interval = frameCount / count;
                if (interval <= 0) { interval = 1; }

                for (int i = 0; i < frameCount; i += interval)
                {
                    capture.PosFrames = i;
                    capture.Read(img);
                    var bitmap = BitmapConverter.ToBitmap(img);
                    var width = bitmap.Width / 10;
                    var height = bitmap.Height / 10;
                    var resizeBitmap = new Bitmap(bitmap, width, height);
                    resizeBitmap.Save($@"{directory}/{i}.png", ImageFormat.Png);
                    Console.WriteLine($"PosFrames={i}");
                }
            }
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}