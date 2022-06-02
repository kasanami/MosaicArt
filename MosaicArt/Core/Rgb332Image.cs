using MessagePack;
using System.Drawing;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// ピクセルの色をRgb332でもつ画像
    /// </summary>
    [MessagePackObject(true)]
    public class Rgb332Image : BytesImage
    {
        public override int Width { get; set; } = 0;
        public override int Height { get; set; } = 0;

        /// <summary>
        /// 1ピクセル1バイト
        /// </summary>
        public const int PixelSize = 1;
        public Rgb332Image()
        {
        }
        public Rgb332Image(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    var rgb = (Rgb332)color;
                    Bytes.Add(rgb);
                }
            }
        }
        public Rgb332Image(Bitmap bitmap, int width, int height) : this(bitmap.Resize(width, height))
        {
        }
        public override Color GetPixel(int x, int y)
        {
            int offset = ((y * Width) + x);
            Rgb332 rgb = Bytes[offset];
            return rgb;
        }
        public override void SetPixel(int x, int y, Color color)
        {
            int offset = ((y * Width) + x);
            Rgb332 rgb = (Rgb332)color;
            Bytes[offset] = rgb.Bits;
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}