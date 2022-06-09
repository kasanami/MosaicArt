using MessagePack;
using System.Drawing;

namespace MosaicArt.Images
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
        public Rgb332Image4x4(Bitmap bitmap) : base(bitmap, _Width, _Height)
        {
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}