using MessagePack;
using System.Drawing;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// Rgb332かつサイズが4x4固定の画像
    /// </summary>
    [MessagePackObject(true)]
    public class Rgb332Image4x4 : Rgb332Image
    {
        const int _Width = 4;
        const int _Height = 4;

        public override int Width { get; set; } = _Width;
        public override int Height { get; set; } = _Height;

        public const int BytesSize = PixelSize * _Width * _Height;

        public Rgb332Image4x4()
        {
        }
        public Rgb332Image4x4(Bitmap bitmap)
        {
            bitmap = bitmap.Resize(Width, Height);
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
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}