using MessagePack;
using System;
using System.Drawing;

namespace MosaicArt.Colors
{
    /// <summary>
    /// HSVの色を表現
    /// ・各要素は0.0～1.0の値
    /// </summary>
    [MessagePackObject(true)]
    public struct Hsv
    {
        #region 定数
        /// <summary>
        /// 各パラメーターが0のインスタンスとして使用する。黒しては使用しない。
        /// </summary>
        public static Hsv Zero => new (0, 0, 0);

        public static Hsv Black => new (0, 0, 0);
        public static Hsv Blue => new (4f / 6f, 1, 1);
        public static Hsv Cyan => new (3f / 6f, 1, 1);
        public static Hsv Gray => new (0, 0, 0.5f);
        public static Hsv Green => new (2f / 6f, 1, 1);
        /// <summary>
        /// English spelling for gray.
        /// </summary>
        public static Hsv Grey => Gray;
        public static Hsv Magenta => new (5f / 6f, 1, 1);
        public static Hsv Red => new (0, 1, 1);
        public static Hsv White => new (0, 0, 1);
        public static Hsv Yellow => new (1f / 6f, 1, 1);
        #endregion 定数

        #region フィールド
        /// <summary>
        /// 色相(Hue)
        /// </summary>
        public float H;
        /// <summary>
        /// 彩度(Saturation・Chroma)
        /// </summary>
        public float S;
        /// <summary>
        /// 明度(Value・Lightness・Brightness)
        /// </summary>
        public float V;
        #endregion フィールド

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="h">色相</param>
        /// <param name="s">彩度</param>
        /// <param name="v">明度</param>
        public Hsv(float h, float s, float v)
        {
            this.H = h;
            this.S = s;
            this.V = v;
        }

        #region operator
        public static explicit operator Hsv(Rgb rgb)
        {
            return Hsv.FromRgb(rgb);
        }
        #endregion operator

        /// <summary>
        /// RGBからHSVへ変換
        /// </summary>
        /// <param name="r">赤色（0.0～1.0）</param>
        /// <param name="g">緑色（0.0～1.0）</param>
        /// <param name="b">青色（0.0～1.0）</param>
        /// <returns>HSV</returns>
        public static Hsv FromRgb(float r, float g, float b)
        {
            float max = System.Math.Max(r, System.Math.Max(g, b));
            float min = System.Math.Min(r, System.Math.Min(g, b));
            float h = max - min;
            if (h > 0.0f)
            {
                if (max == r)
                {
                    h = (g - b) / h;
                    if (h < 0.0f)
                    {
                        h += 6.0f;
                    }
                }
                else if (max == g)
                {
                    h = 2.0f + (b - r) / h;
                }
                else
                {
                    h = 4.0f + (r - g) / h;
                }
                h /= 6;
            }
            float s = (max - min);
            if (max != 0.0f)
                s /= max;
            float v = max;
            return new Hsv(h, s, v);
        }
        public static Hsv FromRgb(Color color)
        {
            return FromRgb(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f);
        }
        public static Hsv FromRgb(Rgb rgb)
        {
            return FromRgb(rgb.R, rgb.G, rgb.B);
        }

        /// <summary>
        /// HSVからRGBへ変換
        /// </summary>
        public static Rgb ToRgb(Hsv hsv)
        {
            return ToRgb(hsv.H, hsv.S, hsv.V);
        }
        /// <summary>
        /// 色相をRGBに変換
        /// </summary>
        /// <param name="h">色相(0.0～1.0)</param>
        /// <param name="s">彩度(0.0～1.0)</param>
        /// <param name="v">明度(0.0～1.0)</param>
        /// <returns>RGB</returns>
        public static Rgb ToRgb(float h, float s, float v)
        {
            float r = v;
            float g = v;
            float b = v;
            if (s > 0)
            {
                h *= 6;
                int i = (int)h;
                float f = h - (float)i;
                switch (i)
                {
                    default:
                    case 0:
                        g *= 1 - s * (1 - f);
                        b *= 1 - s;
                        break;
                    case 1:
                        r *= 1 - s * f;
                        b *= 1 - s;
                        break;
                    case 2:
                        r *= 1 - s;
                        b *= 1 - s * (1 - f);
                        break;
                    case 3:
                        r *= 1 - s;
                        g *= 1 - s * f;
                        break;
                    case 4:
                        r *= 1 - s * (1 - f);
                        g *= 1 - s;
                        break;
                    case 5:
                        g *= 1 - s;
                        b *= 1 - s * f;
                        break;
                }
            }
            return new Rgb(r, g, b);
        }

        #region Object
        public override bool Equals(object? other)
        {
            if (other is Hsv == false)
                return false;
            var otherHsv = (Hsv)other;
            if (this.H != otherHsv.H)
                return false;
            if (this.S != otherHsv.S)
                return false;
            if (this.V != otherHsv.V)
                return false;
            return true;
        }
        /// <summary>
        /// ハッシュコードを生成
        /// </summary>
        public override int GetHashCode()
        {
            int hashCode = H.GetHashCode();
            hashCode ^= S.GetHashCode();
            hashCode ^= V.GetHashCode();
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
            return $"{nameof(Hsv)}{{{H.ToString(format)}, {S.ToString(format)}, {V.ToString(format)}}}";
        }
        #endregion Object
    }
}