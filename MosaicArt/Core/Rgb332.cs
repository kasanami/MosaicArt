using System.Drawing;
using System.Runtime.InteropServices;

namespace MosaicArt.Core
{
    /// <summary>
    /// 8ビットのRGB情報（各値が赤3ビット、緑3ビット、青2ビット）
    /// </summary>
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
        public int R
        {
            get { return (byte)(Bits >> GBBitsCount); }
            set { Bits = (byte)((Bits & ~RBitsMask) | (value << GBBitsCount)); }
        }
        public int G
        {
            get { return (byte)((Bits & GBitsMask) >> BBitsCount); }
            set { Bits = (byte)((Bits & ~GBitsMask) | (value << BBitsCount) & GBitsMask); }
        }
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
        public Rgb332(Color color) : this(color.R, color.G, color.B)
        {
        }
        public Rgb332(Rgb rgb) : this((int)(rgb.R * RMax), (int)(rgb.G * GMax), (int)(rgb.B * BMax))
        {
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
        public static explicit operator Rgb332(Rgb rgb)
        {
            return new Rgb332(rgb);
        }
        #endregion operator
    }
}