using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace MosaicArt.MovieSlicer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine($"引数が不足しています。");
                return;
            }
            var path = args[0];
            int interval;
            int width;
            int height;
            if (int.TryParse(args[1], out interval) == false)
            {
                Console.WriteLine($"args[1]がint型へ変換できません。");
                return;
            }
            if (int.TryParse(args[2], out width) == false)
            {
                Console.WriteLine($"args[2]がint型へ変換できません。");
                return;
            }
            if (int.TryParse(args[3], out height) == false)
            {
                Console.WriteLine($"args[3]がint型へ変換できません。");
                return;
            }
            if (interval <= 0)
            {
                Console.WriteLine($"{nameof(interval)}が0以下です。");
                return;
            }
            if (width <= 0)
            {
                Console.WriteLine($"{nameof(width)}が0以下です。");
                return;
            }
            if (height <= 0)
            {
                Console.WriteLine($"{nameof(width)}が0以下です。");
                return;
            }

            using (var capture = new VideoCapture(path))
            {
                var directory = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path);
                if (Directory.Exists(directory) == false)
                {
                    Directory.CreateDirectory(directory);
                }
                var img = new Mat();
                var frameCount = capture.FrameCount - 1;
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
}