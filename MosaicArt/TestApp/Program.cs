using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MessagePack;
using MosaicArt.Core;
using static MosaicArt.Core.Utility;

namespace MosaicArt.TestApp
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            int divisionsX = 40;
            int divisionsY = 40;

            Console.WriteLine("素材分析");
            ImagesInfo imagesInfo = new ImagesInfo();
            const string DirectoryPath = @"D:\Develop\Projects\MosaicArt\TestData\Resource";
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
            Dictionary<Point, ImageInfo?> bluePrint = new Dictionary<Point, ImageInfo?>();
            int bluePrintWidth;
            int bluePrintHeight;
            var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\400x400.jpg";
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
                        bluePrint.Add(new Point(x, y), nearImageInfo);
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
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}