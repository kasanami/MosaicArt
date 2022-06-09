using MessagePack;
using System.Drawing;

namespace MosaicArt.Images
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 圧縮した画像
    /// </summary>
    [MessagePackObject(true)]
    public class RgbImage4x4 : RgbImage
    {
        const int _Width = 4;
        const int _Height = 4;
        public override int Width { get; set; } = _Width;
        public override int Height { get; set; } = _Height;
        /// <summary>
        /// 1ピクセル3バイト
        /// </summary>
        const int PixelSize = 3;
        public RgbImage4x4()
        {
        }
        public RgbImage4x4(Bitmap bitmap)
        {
            bitmap = bitmap.Resize(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    Bytes.Add(color.R);
                    Bytes.Add(color.G);
                    Bytes.Add(color.B);
                }
            }
        }
        public override Color GetPixel(int x, int y)
        {
            int offset = ((y * Width) + x) * PixelSize;
            var r = Bytes[offset + 0];
            var g = Bytes[offset + 1];
            var b = Bytes[offset + 2];
            return Color.FromArgb(r, g, b);
        }
        public override void SetPixel(int x, int y, Color color)
        {
            int offset = ((y * Width) + x) * PixelSize;
            Bytes[offset + 0] = color.R;
            Bytes[offset + 1] = color.G;
            Bytes[offset + 2] = color.B;
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}