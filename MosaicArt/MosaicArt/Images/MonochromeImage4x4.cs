using MessagePack;
using System.Drawing;

namespace MosaicArt.Images
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 4x4ピクセルのモノクロ画像
    /// </summary>
    [MessagePackObject(true)]
    public class MonochromeImage4x4 : AbstractImage
    {
        public const byte HalfLuminance = 128;

        public const byte PixelCount = 16;

        public override int Width { get; set; } = 4;
        public override int Height { get; set; } = 4;

        /// <summary>
        /// 色情報を0と1のビットで保持する。
        /// 最下位ビットがx=0,y=0の位置
        /// 配列で例えると[y * Width + x]
        /// </summary>
        public UInt16 Bits = 0;
        public MonochromeImage4x4()
        {
        }

        public MonochromeImage4x4(Bitmap bitmap)
        {
            int[] luminanceAry = new int[Width * Height];
            bitmap = bitmap.Resize(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    luminanceAry[y * Width + x] = color.GetLuminance();
                }
            }
            for (int i = 0; i < luminanceAry.Length; i++)
            {
                if (luminanceAry[i] >= HalfLuminance)
                {
                    Bits |= (UInt16)(1 << i);
                }
            }
        }

        public override Color GetPixel(int x, int y)
        {
            var offset = (Width * y) + x;
            var bit = (Bits >> offset) & 1;
            if (bit == 1)
            {
                return Color.White;
            }
            return Color.Black;
        }

        public Color GetPixel(int x, int y, Color zeroColor, Color oneColor)
        {
            var offset = (Width * y) + x;
            var bit = (Bits >> offset) & 1;
            if (bit == 1)
            {
                return oneColor;
            }
            return zeroColor;
        }

        public override void SetPixel(int x, int y, Color color)
        {
            var luminance = color.GetLuminance();
            SetPixel(x, y, luminance >= HalfLuminance);
        }

        public void SetPixel(int x, int y, bool bit)
        {
            var offset = (Width * y) + x;
            UInt16 mask = (UInt16)(1 << offset);
            if (bit)
            {
                Bits |= mask;
            }
            else
            {
                Bits &= (UInt16)~mask;
            }
        }
        /// <summary>
        /// 一致しているピクセルの数
        /// </summary>
        public int MatchCount(MonochromeImage4x4 other)
        {
            return Utility.MatchCount(Bits, other.Bits);
        }
        /// <summary>
        /// Bitmap に変換します。ピクセルの色を指定できます。
        /// </summary>
        public Bitmap ToBitmap(Color zeroColor, Color oneColor)
        {
            var bitmap = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color color = GetPixel(x, y, zeroColor, oneColor);
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}