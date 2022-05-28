using MessagePack;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MosaicArt.Core
{
    /// <summary>
    /// 8ビットのRGB情報（各値が赤3ビット、緑3ビット、青2ビット）
    /// </summary>
    [MessagePackObject(true)]
    public struct Rgb332
    {
        #region 定数
        public const int RBitsCount = 3;
        public const int GBitsCount = 3;
        public const int BBitsCount = 2;
        public const int GBBitsCount = GBitsCount + BBitsCount;
        public const int RBitsMask = 0b1110_0000;
        public const int GBitsMask = 0b0001_1100;
        public const int BBitsMask = 0b0000_0011;
        public const int RMax = 7;
        public const int GMax = 7;
        public const int BMax = 3;

        public static readonly Rgb332 Zero = new Rgb332();
        public static readonly Rgb332 Red = new Rgb332(RMax, 0, 0);
        public static readonly Rgb332 Green = new Rgb332(0, GMax, 0);
        public static readonly Rgb332 Blue = new Rgb332(0, 0, BMax);
        public static readonly Rgb332 White = new Rgb332(RMax, GMax, BMax);
        public static readonly Rgb332 Black = new Rgb332(0, 0, 0);
        #endregion 定数

        #region フィールド
        public byte Bits = 0;
        #endregion フィールド

        #region プロパティ
        [IgnoreMember]
        public int R
        {
            get { return (byte)(Bits >> GBBitsCount); }
            set { Bits = (byte)((Bits & ~RBitsMask) | (value << GBBitsCount)); }
        }
        [IgnoreMember]
        public int G
        {
            get { return (byte)((Bits & GBitsMask) >> BBitsCount); }
            set { Bits = (byte)((Bits & ~GBitsMask) | (value << BBitsCount) & GBitsMask); }
        }
        [IgnoreMember]
        public int B
        {
            get { return (byte)(Bits & BBitsMask); }
            set { Bits = (byte)((Bits & ~BBitsMask) | value & BBitsMask); }
        }
        #endregion プロパティ

        #region コンストラクタ
        public Rgb332(int r, int g, int b)
        {
            R = r;
            G = g;
            B = b;
        }
        public Rgb332(Color color)
        {
            R = Utility.ElasticityBits8To3Array[color.R];
            G = Utility.ElasticityBits8To3Array[color.G];
            B = Utility.ElasticityBits8To2Array[color.B];
        }
        public Rgb332(Rgb rgb) : this((int)Math.Round(rgb.R * RMax), (int)Math.Round(rgb.G * GMax), (int)Math.Round(rgb.B * BMax))
        {
        }
        public Rgb332(byte bits)
        {
            Bits = bits;
        }
        #endregion コンストラクタ

        #region operator
        public static Rgb332 operator +(Rgb332 left, Rgb332 right)
        {
            return new Rgb332(left.R + right.R, left.G + right.G, left.B + right.B);
        }
        public static Rgb332 operator -(Rgb332 left, Rgb332 right)
        {
            return new Rgb332(left.R - right.R, left.G - right.G, left.B - right.B);
        }

        public static explicit operator Rgb332(Color color)
        {
            return new Rgb332(color);
        }
        public static implicit operator Color(Rgb332 rgb)
        {
            // 3ビットを8ビットに拡張する。(0b00000111 →　0b111_111_11)
            // 2ビットを8ビットに拡張する。(0b00000011 →　0b11_11_11_11)
            var r = Utility.ElasticityBits8To3Array[rgb.R];
            var g = Utility.ElasticityBits8To3Array[rgb.G];
            var b = Utility.ElasticityBits8To2Array[rgb.B];
            return Color.FromArgb(r, g, b);
        }
        public static explicit operator Rgb332(Rgb rgb)
        {
            return new Rgb332(rgb);
        }
        public static implicit operator byte(Rgb332 rgb)
        {
            return rgb.Bits;
        }
        public static implicit operator Rgb332(byte bits)
        {
            return new Rgb332(bits);
        }
        #endregion operator
    }
}