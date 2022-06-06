using MessagePack;
using System.Drawing;

namespace MosaicArt.Core
{
    [MessagePackObject(true)]
    public class BytesImage : AbstractImage, IEquatable<BytesImage>
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
        /// <summary>
        /// バイト配列がすべて同じならtrueを返す。
        /// </summary>
        public override bool Equals(object? obj)
        {
            return Equals(obj as BytesImage);
        }
        /// <summary>
        /// バイト配列がすべて同じならtrueを返す。
        /// </summary>
        public bool Equals(BytesImage? other)
        {
            if (other == null)
            {
                return false;
            }
            if (Width != other.Width)
            {
                return false;
            }
            if (Height != other.Height)
            {
                return false;
            }
            if (Bytes.Count != other.Bytes.Count)
            {
                return false;
            }
            return Bytes.SequenceEqual(other.Bytes);
        }

        public override int GetHashCode()
        {
            return Bytes.GetHashCode();
        }
    }
}