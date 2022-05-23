﻿using System.Drawing;

namespace Core
{
    /// <summary>
    /// 色
    /// </summary>
    public struct Rgb
    {
        #region 定数
        public static Rgb Black { get { return new Rgb(0, 0, 0); } }
        public static Rgb Blue { get { return new Rgb(0, 0, 1); } }
        public static Rgb Cyan { get { return new Rgb(0, 1, 1); } }
        public static Rgb Gray { get { return new Rgb(0.5f, 0.5f, 0.5f); } }
        public static Rgb Green { get { return new Rgb(0, 1, 0); } }
        /// <summary>
        /// English spelling for gray.
        /// </summary>
        public static Rgb Grey { get { return Gray; } }
        public static Rgb Magenta { get { return new Rgb(1, 0, 1); } }
        public static Rgb Red { get { return new Rgb(1, 0, 0); } }
        public static Rgb White { get { return new Rgb(1, 1, 1); } }
        public static Rgb Yellow { get { return new Rgb(1, 1, 0); } }
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
        #endregion operator

        #region Object
        public override bool Equals(object? other)
        {
            if (other is Rgb == false)
                return false;
            var otherRgb = (Rgb)other;
            if (this.R != otherRgb.R)
                return false;
            if (this.G != otherRgb.G)
                return false;
            if (this.B != otherRgb.B)
                return false;
            return true;
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
    }
}