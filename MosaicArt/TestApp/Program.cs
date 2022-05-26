using System.Drawing;
using System.Drawing.Imaging;
using static Core.Utility;

namespace TestApp
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var path = @"D:\Develop\Projects\MosaicArt\TestData\Target1.png";
            //ImageSlicer(path);
            // 比較
        }

        static void ImageSlicer(string imagePath)
        {
            Bitmap bitmap = new Bitmap(imagePath);
            var width = bitmap.Width;
            var height = bitmap.Height;
            var w = bitmap.Width / 60;
            var h = bitmap.Height / 60;
            for (int y = 0; y < height; y += h)
            {
                for (int x = 0; x < width; x += w)
                {
                    var rect = new Rectangle(x, y, w, h);
                    var clippedBitmap = bitmap.Clone(rect, bitmap.PixelFormat);
                    clippedBitmap.Save($@"{imagePath}_({x},{y}).png", ImageFormat.Png);
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