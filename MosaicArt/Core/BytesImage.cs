using MessagePack;
using System.Drawing;

namespace MosaicArt.Core
{
    [MessagePackObject(true)]
    public class BytesImage : AbstractImage
    {
        public override int Width { get; set; } = 0;
        public override int Height { get; set; } = 0;
        public List<byte> Bytes = new();

        public override Color GetPixel(int x, int y)
        {
            throw new NotImplementedException();
        }

        public override void SetPixel(int x, int y, Color color)
        {
            throw new NotImplementedException();
        }
    }
}