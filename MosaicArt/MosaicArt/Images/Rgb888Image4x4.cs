using MessagePack;
using System.Drawing;

namespace MosaicArt.Images
{
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

        public Rgb888Image4x4()
        {
        }
        public Rgb888Image4x4(Bitmap bitmap) : base(bitmap, _Width, _Height)
        {
        }
    }
}