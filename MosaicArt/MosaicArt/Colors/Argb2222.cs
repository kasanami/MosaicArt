using MessagePack;
using System.Drawing;
using System.Runtime.InteropServices;

namespace MosaicArt.Colors
{
    /// <summary>
    /// 8ビットのARGB情報（各値が2ビットとなる）
    /// </summary>
    [MessagePackObject(true)]
    public struct Argb2222
    {
        #region 定数
        public const int ABitsCount = 2;
        public const int RBitsCount = 2;
        public const int GBitsCount = 2;
        public const int BBitsCount = 2;
        public const int ABitsShift = RBitsCount + GBitsCount + BBitsCount;
        public const int RBitsShift = GBitsCount + BBitsCount;
        public const int GBitsShift = BBitsCount;
        public const int BBitsShift = 0;
        public const int ABitsMask = 0b1100_0000;
        public const int RBitsMask = 0b0011_0000;
        public const int GBitsMask = 0b0000_1100;
        public const int BBitsMask = 0b0000_0011;
        public const int AMax = 3;
        public const int RMax = 3;
        public const int GMax = 3;
        public const int BMax = 3;

        public static readonly Argb2222 Zero = new Argb2222();
        public static readonly Argb2222 Red = new Argb2222(RMax, 0, 0);
        public static readonly Argb2222 Green = new Argb2222(0, GMax, 0);
        public static readonly Argb2222 Blue = new Argb2222(0, 0, BMax);
        public static readonly Argb2222 White = new Argb2222(RMax, GMax, BMax);
        public static readonly Argb2222 Black = new Argb2222(0, 0, 0);
        #endregion 定数

        #region フィールド
        public byte Bits = 0;
        #endregion フィールド

        #region プロパティ
        [IgnoreMember]
        public int A
        {
            get { return (byte)((Bits & ABitsMask) >> ABitsShift); }
            set { Bits = (byte)((Bits & ~ABitsMask) | (value << ABitsShift) & ABitsMask); }
        }
        [IgnoreMember]
        public int R
        {
            get { return (byte)((Bits & RBitsMask) >> RBitsShift); }
            set { Bits = (byte)((Bits & ~RBitsMask) | (value << RBitsShift) & RBitsMask); }
        }
        [IgnoreMember]
        public int G
        {
            get { return (byte)((Bits & GBitsMask) >> GBitsShift); }
            set { Bits = (byte)((Bits & ~GBitsMask) | (value << GBitsShift) & GBitsMask); }
        }
        [IgnoreMember]
        public int B
        {
            get { return (byte)((Bits & BBitsMask) >> BBitsShift); }
            set { Bits = (byte)((Bits & ~BBitsMask) | (value << BBitsShift) & BBitsMask); }
        }
        #endregion プロパティ

        #region コンストラクタ
        public Argb2222(int a, int r, int g, int b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
        public Argb2222(int r, int g, int b)
        {
            A = AMax;
            R = r;
            G = g;
            B = b;
        }
        public Argb2222(Color color)
        {
            A = Utility.ElasticityBits8To2Array[color.A];
            R = Utility.ElasticityBits8To2Array[color.R];
            G = Utility.ElasticityBits8To2Array[color.G];
            B = Utility.ElasticityBits8To2Array[color.B];
        }
        public Argb2222(Rgb rgb) : this(AMax, (int)Math.Round(rgb.R * RMax), (int)Math.Round(rgb.G * GMax), (int)Math.Round(rgb.B * BMax))
        {
        }
        public Argb2222(byte bits)
        {
            Bits = bits;
        }
        #endregion コンストラクタ

        #region operator
        public static Argb2222 operator +(Argb2222 left, Argb2222 right)
        {
            return new Argb2222(left.A + right.A, left.R + right.R, left.G + right.G, left.B + right.B);
        }
        public static Argb2222 operator -(Argb2222 left, Argb2222 right)
        {
            return new Argb2222(left.A - right.A, left.R - right.R, left.G - right.G, left.B - right.B);
        }

        public static explicit operator Argb2222(Color color)
        {
            return new Argb2222(color);
        }
        public static implicit operator Color(Argb2222 rgb)
        {
            var a = Utility.ElasticityBits2To8Array[rgb.A];
            var r = Utility.ElasticityBits2To8Array[rgb.R];
            var g = Utility.ElasticityBits2To8Array[rgb.G];
            var b = Utility.ElasticityBits2To8Array[rgb.B];
            return Color.FromArgb(a, r, g, b);
        }
        public static explicit operator Argb2222(Rgb rgb)
        {
            return new Argb2222(rgb);
        }
        public static implicit operator byte(Argb2222 rgb)
        {
            return rgb.Bits;
        }
        public static implicit operator Argb2222(byte bits)
        {
            return new Argb2222(bits);
        }
        #endregion operator
    }
}