using MessagePack;
using System.Drawing;

namespace MosaicArt.Images
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// HSV,HSBの明るさ情報だけの画像
    /// クラス名はValueだと意味がわからないのでBrightnessとした。
    /// </summary>
    [MessagePackObject(true)]
    public class BrightnessImage : BytesImage
    {
        /// <summary>
        /// 1ピクセル1バイト
        /// </summary>
        const int PixelSize = 1;
        public BrightnessImage()
        {
        }
        public BrightnessImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    Bytes.Add(ToBrightness(color));
                }
            }
        }
        public BrightnessImage(Bitmap bitmap, int width, int height) : this(bitmap.Resize(width, height))
        {
        }
        public override Color GetPixel(int x, int y)
        {
            int offset = GetPixelOffset(x, y, PixelSize);
            var brightness = Bytes[offset];
            return Color.FromArgb(brightness, brightness, brightness);
        }
        public override void SetPixel(int x, int y, Color color)
        {
            int offset = GetPixelOffset(x, y, PixelSize);
            Bytes[offset] = ToBrightness(color);
        }
        public static byte ToBrightness(Color color)
        {
            return Math.Max(color.R, Math.Max(color.G, color.B));
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}