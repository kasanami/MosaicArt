using System.Drawing;

namespace Core
{
    /// <summary>
    /// 色
    /// </summary>
    public struct Rgb
    {
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
            R = color.R;
            G = color.G;
            B = color.B;
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
    }
}