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
        public RgbImage4x4(Bitmap bitmap) : base(bitmap, _Width, _Height)
        {
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}