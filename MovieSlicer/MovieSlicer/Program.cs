using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenCvSharp;
using OpenCvSharp.Extensions;

namespace MovieSlicer
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var path = @".mp4";
            using (var capture = new VideoCapture(path))
            {
                var img = new Mat();
                var frameCount=capture.FrameCount-2;
                for (int i = 0; i < frameCount; i += 10)
                {
                    capture.PosFrames = i;
                    capture.Read(img);
                    var bitmap = BitmapConverter.ToBitmap(img);
                    var resizeBitmap = new Bitmap(bitmap, 16, 9);
                    resizeBitmap.Save($@"{path}_{i}.png", ImageFormat.Png);
                    Console.WriteLine($"PosFrames={i}");
                }
            }
        }
    }
}