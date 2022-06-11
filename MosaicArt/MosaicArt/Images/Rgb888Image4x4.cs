using MessagePack;
using System.Drawing;

namespace MosaicArt.Images
{
#pragma warning disable IDE0051 // 使用されていないプライベート メンバーを削除する
    /// <summary>
    /// 圧縮した画像
    /// </summary>
    [MessagePackObject(true)]
    public class Rgb888Image4x4 : Rgb888Image
    {
        const int _Width = 4;
        const int _Height = 4;
        public override int Width { get; set; } = _Width;
        public override int Height { get; set; } = _Height;
        /// <summary>
        /// 1ピクセル3バイト
        /// </summary>
        const int PixelSize = 3;
        public Rgb888Image4x4()
        {
        }
        public Rgb888Image4x4(Bitmap bitmap) : base(bitmap, _Width, _Height)
        {
        }
    }
#pragma warning restore IDE0051 // 使用されていないプライベート メンバーを削除する
}