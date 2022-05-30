using MessagePack;
using System.Drawing;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 輝度画像
    /// </summary>
    [MessagePackObject(true)]
    public class LuminanceImage : BytesImage
    {
        /// <summary>
        /// 1ピクセル1バイト
        /// </summary>
        const int PixelSize = 1;
        public LuminanceImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    Bytes.Add(color.GetLuminance());
                }
            }
        }
        public LuminanceImage(Bitmap bitmap, int width, int height) : this(bitmap.Resize(width, height))
        {
        }
        public override Color GetPixel(int x, int y)
        {
            int offset = ((y * Width) + x);
            var l = Bytes[offset];
            return Color.FromArgb(l, l, l);
        }
        public override void SetPixel(int x, int y, Color color)
        {
            int offset = ((y * Width) + x);
            Bytes[offset] = color.GetLuminance();
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}