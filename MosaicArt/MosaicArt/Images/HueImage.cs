using MessagePack;
using MosaicArt.Colors;
using System.Drawing;

namespace MosaicArt.Images
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// HSV,HSBの色相情報だけの画像
    /// </summary>
    [MessagePackObject(true)]
    public class HueImage : BytesImage
    {
        /// <summary>
        /// 1ピクセル1バイト
        /// </summary>
        const int PixelSize = 1;
        public HueImage()
        {
        }
        public HueImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    Bytes.Add(ToHue(color));
                }
            }
        }
        public HueImage(Bitmap bitmap, int width, int height) : this(bitmap.Resize(width, height))
        {
        }
        public override Color GetPixel(int x, int y)
        {
            int offset = GetPixelOffset(x, y, PixelSize);
            var hue = Bytes[offset] / 360f;
            var rgb = Hsv.ToRgb(hue, 1, 1);
            return (Color)rgb;
        }
        public override void SetPixel(int x, int y, Color color)
        {
            int offset = GetPixelOffset(x, y, PixelSize);
            Bytes[offset] = ToHue(color);
        }
        public static byte ToHue(Color color)
        {
            // GetHue()は0～360なので0～1.0に変換
            var rate = color.GetHue() / 360;
            return (byte)Math.Round(rate * 255);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}