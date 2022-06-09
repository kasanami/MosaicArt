using MessagePack;
using System.Drawing;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 圧縮した画像
    /// </summary>
    [MessagePackObject(true)]
    public class BrightnessImage4x4 : BrightnessImage
    {
        const int _Width = 4;
        const int _Height = 4;
        public override int Width { get; set; } = _Width;
        public override int Height { get; set; } = _Height;

        public const int BytesSize = PixelSize * _Width * _Height;
        /// <summary>
        /// 1ピクセル3バイト
        /// </summary>
        const int PixelSize = 3;
        public BrightnessImage4x4()
        {
        }
        public BrightnessImage4x4(Bitmap bitmap) : base(bitmap, _Width, _Height)
        {
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}