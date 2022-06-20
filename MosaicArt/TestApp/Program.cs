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
    using BluePrint = Dictionary<System.Drawing.Point, ImageInfo?>;
    using Pieces = Dictionary<System.Drawing.Point, Bitmap>;
#pragma warning disable CA1416 // プラットフォームの互換性を検証
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
    static class Program
    {
        static DateTime startTime;
        static Random random;
        static Bitmap targetImage;
        static ParallelOptions parallelOptions;
        static string destinationPath;
        [STAThread]
        static void Main(string[] args)
        {
            startTime = DateTime.Now;
            Console.WriteLine($"開始 {startTime}");

            Parameters param = new();
            param.MaxDegreeOfParallelism = 8;
            param.RandomSeed = 123456789;
            param.ResourceDirectoryPath = @"D:\Develop\Projects\MosaicArt\TestData\Resource";
            param.MovieSliceCount = 300;
            param.TargetImagePath = @"D:\Develop\Projects\MosaicArt\TestData\Target0\YoutubeIcon4000x4000.jpg";
            param.DivisionsX = 100;
            param.DivisionsY = 100;

            Console.WriteLine($"{nameof(param.TargetImagePath)}={param.TargetImagePath}");
            random = new Random(param.RandomSeed);
            targetImage = new(param.TargetImagePath);
            // パラメーターチェック
            if (targetImage.Width % param.DivisionsX != 0)
            {
                Console.WriteLine($"{nameof(targetImage.Width)}が{param.DivisionsX}の倍数ではありません。");
                return;
            }
            if (targetImage.Height % param.DivisionsY != 0)
            {
                Console.WriteLine($"{nameof(targetImage.Height)}が{param.DivisionsY}の倍数ではありません。");
                return;
            }

            parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = param.MaxDegreeOfParallelism };
            bool IsCompulsoryCreateResource = false;// 素材を再作成するならtrue


            Console.WriteLine("素材作成");
            _CreateResource(param, IsCompulsoryCreateResource);

            Console.WriteLine("素材分析");
            ImagesInfo imagesInfo = _CreateImagesInfo(param, IsCompulsoryCreateResource);
            if (imagesInfo.ImageInfos.Count <= 0)
            {
                Console.WriteLine("素材がありません");
                return;
            }

#if false// 品質下がるのでOFF
            Console.WriteLine("素材整理");
            {
                // ほとんど同じ画像は除外
                imagesInfo.RemoveDuplicates();
            }
#endif

            Console.WriteLine($"ピース作成");
            Pieces pieces = _CreatePieces(param);

            Console.WriteLine($"現在の処理時間:{(DateTime.Now - startTime)}");

            Console.WriteLine("分析・設計図作成");
            var bluePrint = _CreateBluePrint(param, pieces, imagesInfo);

            // 保存パス
            {
                destinationPath = Path.GetDirectoryName(param.TargetImagePath);
                destinationPath += "/";
                destinationPath += Path.GetFileNameWithoutExtension(param.TargetImagePath);
                destinationPath += "_MosaicArt";
                destinationPath += DateTime.Now.ToString("(yyyyMMdd_HHmmss)");
                destinationPath += ".png";
            }

            Console.WriteLine("モザイクアート生成");
            _CreateMosaicArt(param, bluePrint);

            DateTime endTime = DateTime.Now;
            Console.WriteLine($"完了 処理時間:{(endTime - startTime)}");
            // レポート保存
            {
                Report report = new();
                report.StartTime = startTime;
                report.EndTime = endTime;
                report.ElapsedTime = endTime - startTime;
                report.MiniImageWidth = ImageInfo.MiniImageWidth;
                report.MiniImageHeight = ImageInfo.MiniImageHeight;
                report.FastCompareCount = ImagesInfo.FastCompareCount;
                report.Parameters = param;

                destinationPath += ".report.json";
                report.Save(destinationPath);
            }
        }

        private static void _CreateResource(Parameters param, bool isCompulsoryCreateResource)
        {
            var files = Directory.GetFiles(param.ResourceDirectoryPath, "*.mp4");
            foreach (var file in files)
            {
                var directory = Path.GetDirectoryName(file) + "/" + Path.GetFileNameWithoutExtension(file);
                if (Directory.Exists(directory) == false || isCompulsoryCreateResource)
                {
                    MovieSlicer(file, param.MovieSliceCount);
                }
            }
        }

        private static ImagesInfo _CreateImagesInfo(Parameters param, bool isCompulsoryCreateResource)
        {
            ImagesInfo imagesInfo = new();
            var directories = Directory.GetDirectories(param.ResourceDirectoryPath);
            foreach (var d in directories)
            {
                bool newCreate = true;
                // 既存のImagesInfoを確認
                // 無いorVersionが古かったら作成する。
                var path = d + ImagesInfo.PathExtension;
                if (File.Exists(path) && isCompulsoryCreateResource == false)
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
            return imagesInfo;
        }

        private static Pieces _CreatePieces(Parameters param)
        {
            Pieces pieces = new();
            var directoryPath = Path.GetDirectoryName(param.TargetImagePath) + "/" +
                Path.GetFileNameWithoutExtension(param.TargetImagePath) +
                $"_{param.DivisionsX}x{param.DivisionsY}";
            Directory.CreateDirectory(directoryPath);
            var width = targetImage.Width;
            var height = targetImage.Height;
            var w = width / param.DivisionsX;
            var h = height / param.DivisionsY;

            for (int y = 0; y < height; y += h)
            {
                for (int x = 0; x < width; x += w)
                {
                    //Console.WriteLine($"{x}x{y}");
                    Bitmap pieceImage = null;
                    var path = directoryPath + $"/{x}_{y}.png";
                    if (File.Exists(path))
                    {
                        pieceImage = new(path);
                    }
                    if (pieceImage == null)
                    {
                        pieceImage = targetImage.Clip(new Rectangle(x, y, w, h));
                        pieceImage.Save(path, ImageFormat.Png);
                    }
                    var point = new System.Drawing.Point(x, y);
                    pieces.Add(point, pieceImage);
                }
            }
            return pieces;
        }

        private static BluePrint _CreateBluePrint(Parameters param, Pieces pieces, ImagesInfo imagesInfo)
        {
            BluePrint bluePrint = new();
            var width = targetImage.Width;
            var height = targetImage.Height;
            var w = width / param.DivisionsX;
            var h = height / param.DivisionsY;

            var points = pieces.Keys.ToList();
            Console.WriteLine($"{nameof(points.Count)}={points.Count}");
            Shuffle(points, random);
#if ENABLE_PARALLELS
            Parallel.ForEach(points, parallelOptions, point =>
            {
#else
                int i = 0;
                foreach (var point in points)
                {
#endif
                var x = point.X;
                var y = point.Y;
                //Console.WriteLine($"{i}:{x}x{y}");
                Bitmap clippedBitmap;
                lock (pieces)
                {
                    clippedBitmap = pieces[point];
                }
                var imageInfo = new ImageInfo(clippedBitmap);
                imageInfo.MakeComparisonImage();
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
            return bluePrint;
        }

        private static void _CreateMosaicArt(Parameters param, BluePrint bluePrint)
        {
            // モザイクアート生成
            Bitmap bitmap = new Bitmap(targetImage.Width, targetImage.Height);
            var w = targetImage.Width / param.DivisionsX;
            var h = targetImage.Height / param.DivisionsY;
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
        static void ImageSlicer(string imagePath, int divisionsX, int divisionsY)
        {
            var directoryPath = Path.GetDirectoryName(imagePath) + "/" + Path.GetFileNameWithoutExtension(imagePath) + $"_{divisionsX}x{divisionsY}";
            Directory.CreateDirectory(directoryPath);
            Bitmap bitmap = new(imagePath);
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
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}