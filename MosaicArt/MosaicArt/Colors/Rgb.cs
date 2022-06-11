using MessagePack;
using System.Drawing;

namespace MosaicArt.Colors
{
    /// <summary>
    /// 色
    /// </summary>
    [MessagePackObject(true)]
    public struct Rgb : IEquatable<Rgb>
    {
        #region 定数
        /// <summary>
        /// 各パラメーターが0のインスタンスとして使用する。黒しては使用しない。
        /// </summary>
        public static Rgb Zero => new (0, 0, 0);

        public static Rgb Black => new (0, 0, 0);
        public static Rgb Blue => new (0, 0, 1);
        public static Rgb Cyan => new (0, 1, 1);
        public static Rgb Gray => new (0.5f, 0.5f, 0.5f);
        public static Rgb Green => new (0, 1, 0);
        /// <summary>
        /// English spelling for gray.
        /// </summary>
        public static Rgb Grey => Gray;
        public static Rgb Magenta => new (1, 0, 1);
        public static Rgb Red => new (1, 0, 0);
        public static Rgb White => new (1, 1, 1);
        public static Rgb Yellow => new (1, 1, 0);
        #endregion 定数

        public float R;
        public float G;
        public float B;

        public Rgb(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }
        public Rgb(Color color)
        {
            R = color.R / 255f;
            G = color.G / 255f;
            B = color.B / 255f;
        }

        #region operator
        public static bool operator ==(Rgb left, Rgb right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(Rgb left, Rgb right)
        {
            return left.Equals(right) == false;
        }
        public static Rgb operator +(Rgb left, Rgb right)
        {
            return new Rgb(left.R + right.R, left.G + right.G, left.B + right.B);
        }
        public static Rgb operator -(Rgb left, Rgb right)
        {
            return new Rgb(left.R - right.R, left.G - right.G, left.B - right.B);
        }
        public static Rgb operator *(Rgb left, Rgb right)
        {
            return new Rgb(left.R * right.R, left.G * right.G, left.B * right.B);
        }
        public static Rgb operator /(Rgb left, Rgb right)
        {
            return new Rgb(left.R / right.R, left.G / right.G, left.B / right.B);
        }
        public static Rgb operator /(Rgb left, int right)
        {
            return new Rgb(left.R / right, left.G / right, left.B / right);
        }
        public static Rgb operator /(Rgb left, float right)
        {
            return new Rgb(left.R / right, left.G / right, left.B / right);
        }

        public static implicit operator Rgb(Color color)
        {
            return new Rgb(color);
        }

        public static explicit operator Rgb(Hsv hsv)
        {
            return Hsv.ToRgb(hsv);
        }

        public static explicit operator Color(Rgb rgb)
        {
            int r = (int)Math.Round(rgb.R * 255);
            int g = (int)Math.Round(rgb.G * 255);
            var b = (int)Math.Round(rgb.B * 255);
            r = Math.Clamp(r, 0, 255);
            g = Math.Clamp(g, 0, 255);
            b = Math.Clamp(b, 0, 255);
            return Color.FromArgb(r, g, b);
        }
        #endregion operator

        #region Object
        public override bool Equals(object? other)
        {
            if (other is Rgb == false)
            {
                return false;
            }
            return Equals((Rgb)other);
        }
        /// <summary>
        /// ハッシュコードを生成
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = R.GetHashCode();
            hashCode ^= G.GetHashCode();
            hashCode ^= B.GetHashCode();
            return hashCode;
        }
        /// <summary>
        /// 文字列に変換
        /// ・UnityEngine.Colorに合わせてformatは"0.000"
        /// </summary>
        public override string ToString()
        {
            return ToString("0.000");
        }
        public string ToString(string format)
        {
            return $"{nameof(Rgb)}{{{R.ToString(format)}, {G.ToString(format)}, {B.ToString(format)}}}";
        }
        #endregion Object

        public bool Equals(Rgb other)
        {
            if (this.R != other.R)
                return false;
            if (this.G != other.G)
                return false;
            if (this.B != other.B)
                return false;
            return true;
        }
    }
}