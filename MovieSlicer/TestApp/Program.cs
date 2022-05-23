using System.Drawing;
using System.Drawing.Imaging;

namespace TestApp
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var path = @"";
            Bitmap bitmap = new Bitmap(path);
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
                    clippedBitmap.Save($@"{path}_({x},{y}).png", ImageFormat.Png);
                }
            }
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}