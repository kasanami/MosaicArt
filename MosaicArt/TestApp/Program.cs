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
            var path = @"D:\Develop\Projects\MosaicArt\TestData\20220427 【original animation MV】マリン出航！！【hololive 宝鐘マリン】.mp4_snapshot_01.15_[2022.05.21_18.15.21].png";
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