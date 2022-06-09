using System.Drawing;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    public static class BitmapExtensions
    {
        public static Bitmap Clip(this Bitmap source, Rectangle rectangle)
        {
            return source.Clone(rectangle, source.PixelFormat);
        }

        public static Bitmap Resize(this Bitmap source, Size size)
        {
            return new Bitmap(source, size);
        }

        public static Bitmap Resize(this Bitmap source, int width, int height)
        {
            return new Bitmap(source, width, height);
        }

        public static Color AverageColor(this Bitmap bitmap)
        {
            double r = 0;
            double g = 0;
            double b = 0;
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    r += color.R;
                    g += color.G;
                    b += color.B;
                }
            }
            long area = bitmap.Width * (long)bitmap.Height;
            int ri = (int)Math.Round(r / area);
            int gi = (int)Math.Round(g / area);
            int bi = (int)Math.Round(b / area);
            return Color.FromArgb(ri, gi, bi);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}
