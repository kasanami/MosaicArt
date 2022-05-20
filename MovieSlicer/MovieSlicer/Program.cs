using System;
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
            using (var capture = new VideoCapture(@"d:\test.mp4"))
            {
                var img = new Mat();
                capture.Read(img);
                BitmapConverter.ToBitmap(img).Save(@"d:\test.jpg");
            }

        }
    }
}
