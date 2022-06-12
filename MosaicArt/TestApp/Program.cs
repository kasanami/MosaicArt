#define ENABLE_PARALLELS
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MessagePack;
using MosaicArt.Images;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using static MosaicArt.Utility;

namespace MosaicArt.TestApp
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            DateTime startTime = DateTime.Now;
            Console.WriteLine($"開始 {startTime}");

            Parameters param = new();
            param.MaxDegreeOfParallelism = 8;
            param.RandomSeed = 123456789;
            param.ResourceDirectoryPath = @"D:\Develop\Projects\MosaicArt\TestData\Resource";
            param.MovieSliceCount = 300;
            param.TargetImagePath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon4000x4000.jpg";
            param.DivisionsX = 100;
            param.DivisionsY = 100;

            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = param.MaxDegreeOfParallelism };
            bool IsRemakeResource = false;// 素材を再作成するならtrue
            var random = new Random(param.RandomSeed);

            //var targetPath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\マリン出航！_3500x2000.png";
            Console.WriteLine($"{nameof(param.TargetImagePath)}={param.TargetImagePath}");

#if false
            {
                Bitmap bitmap = new Bitmap(@"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon.jpg");
                BrightnessImage brightnessImage = new(bitmap);
                brightnessImage.Save(@"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon.brightness.png", ImageFormat.Png);
                HueImage hueImage = new(bitmap);
                hueImage.Save(@"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon.hue.png", ImageFormat.Png);
                RgbImage rgbImage = new(bitmap);
                rgbImage.Save(@"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon.rgb.png", ImageFormat.Png);
                Rgb332Image rgb332Image = new(bitmap);
                rgb332Image.Save(@"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon.rgb332.png", ImageFormat.Png);
            }
#endif

            Console.WriteLine("素材作成");
            {
                var files = Directory.GetFiles(param.ResourceDirectoryPath, "*.mp4");
                foreach (var file in files)
                {
                    var directory = Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file);
                    if (Directory.Exists(directory) == false || IsRemakeResource)
                    {
                        MovieSlicer(file, param.MovieSliceCount);
                    }
                }
            }

            Console.WriteLine("素材分析");
            ImagesInfo imagesInfo = new ();
            var directories = Directory.GetDirectories(param.ResourceDirectoryPath);
            foreach (var d in directories)
            {
                bool newCreate = true;
                // 既存のImagesInfoを確認
                // 無いorVersionが古かったら作成する。
                var path = d + ImagesInfo.PathExtension;
                if (File.Exists(path) && IsRemakeResource == false)
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

#if false// 品質下がるのでOFF
            Console.WriteLine("素材整理");
            {
                // ほとんど同じ画像は除外
                imagesInfo.RemoveDuplicates();
            }
#endif

            // 設計図
            Dictionary<System.Drawing.Point, ImageInfo?> bluePrint = new();
            int bluePrintWidth;
            int bluePrintHeight;
            // targetの分析、resourceと比較
            Console.WriteLine("分析・設計図作成");
            try
            {
                Bitmap bitmap = new (param.TargetImagePath);
                var width = bitmap.Width;
                var height = bitmap.Height;
                var w = width / param.DivisionsX;
                var h = height / param.DivisionsY;
                if (width % param.DivisionsX != 0)
                {
                    Console.WriteLine($"{nameof(width)}が{param.DivisionsX}の倍数ではありません。");
                    return;
                }
                if (height % param.DivisionsY != 0)
                {
                    Console.WriteLine($"{nameof(height)}が{param.DivisionsY}の倍数ではありません。");
                    return;
                }

                List<System.Drawing.Point> points = new();
                {
                    for (int y = 0; y < height; y += h)
                    {
                        for (int x = 0; x < width; x += w)
                        {
                            points.Add(new System.Drawing.Point(x, y));
                        }
                    }
                    Shuffle(points, random);
                }
                Console.WriteLine($"{nameof(points.Count)}={points.Count}");
#if ENABLE_PARALLELS
                Parallel.For(0, points.Count, parallelOptions, i =>
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
                ConsoleWrite(ex);
                return;
            }
            // 保存パス
            var destinationPath = Path.GetDirectoryName(param.TargetImagePath);
            {
                destinationPath += "/";
                destinationPath += Path.GetFileNameWithoutExtension(param.TargetImagePath);
                destinationPath += "_MosaicArt";
                destinationPath += DateTime.Now.ToString("(yyyyMMdd_HHmmss)");
                destinationPath += ".png";
            }
            // モザイクアート生成
            Console.WriteLine("モザイクアート生成");
            {
                Bitmap bitmap = new (param.TargetImagePath);
                //Bitmap bitmap = new Bitmap(bluePrintWidth, bluePrintHeight);
                var w = bitmap.Width / param.DivisionsX;
                var h = bitmap.Height / param.DivisionsY;
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
                bitmap.Save(destinationPath, ImageFormat.Png);
            }
            DateTime endTime = DateTime.Now;
            Console.WriteLine($"完了 処理時間:{(endTime - startTime)}");
            // レポート保存
            {
                Report report = new();
                report.StartTime = startTime;
                report.EndTime = endTime;
                report.ElapsedTime = endTime - startTime;
                report.MiniImageWidth = 8;
                report.MiniImageHeight = 8;
                report.Parameters = param;

                destinationPath += ".report.json";
                report.Save(destinationPath);
            }
        }

        static void ImageSlicer(string imagePath, int divisionsX, int divisionsY)
        {
            var directoryPath = Path.GetDirectoryName(imagePath) + "/" + Path.GetFileNameWithoutExtension(imagePath);
            Directory.CreateDirectory(directoryPath);
            Bitmap bitmap = new (imagePath);
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
        /// <returns>成功したらtrueを返す。</returns>
        static bool MovieSlicer(string path, int count)
        {
            Console.WriteLine($"{path}, {count}");
            try
            {
                using var capture = new VideoCapture(path);
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
            catch (Exception ex)
            {
                ConsoleWrite(ex);
                return false;
            }
            return true;
        }

        static void ConsoleWrite(Exception ex)
        {
            Console.WriteLine("ERROR!");
            Console.WriteLine($"Message:{ex.Message}");
            Console.WriteLine($"StackTrace:{ex.StackTrace}");
        }
    }
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}