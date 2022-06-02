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
            path = @"D:\Develop\Projects\MosaicArt\TestData\Resource\20220427 【#マリン出航3DLIVE】ゲストとワイワイ！Marine Set Sail!! Concert!!【ホロライブ 宝鐘マリン】.mp4";

            Console.WriteLine($"{nameof(path)}={path}");
            int count;
            int width;
            int height;
            if (int.TryParse(args[1], out count) == false)
            {
                Console.WriteLine($"args[1]がint型へ変換できません。");
                return;
            }
            Console.WriteLine($"{nameof(count)}={count}");
            if (int.TryParse(args[2], out width) == false)
            {
                Console.WriteLine($"args[2]がint型へ変換できません。");
                return;
            }
            Console.WriteLine($"{nameof(width)}={width}");
            if (int.TryParse(args[3], out height) == false)
            {
                Console.WriteLine($"args[3]がint型へ変換できません。");
                return;
            }
            Console.WriteLine($"{nameof(height)}={height}");
            if (count <= 0)
            {
                Console.WriteLine($"{nameof(count)}が0以下です。");
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
}