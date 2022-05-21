using System;
using System.Drawing;

namespace Core
{
    /// <summary>
    /// HSVの色を表現
    /// ・各要素は0.0～1.0の値
    /// </summary>
    public struct Hsv
    {
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

        /// <summary>
        /// RGBAからHSVへ変換
        /// ・不透明度は無くなる
        /// </summary>
        public static Hsv FromRgb(Color rgb)
        {
            float r = rgb.R;
            float g = rgb.G;
            float b = rgb.B;
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

        /// <summary>
        /// HSVからRGBAへ変換
        /// ・不透明度は1になる。
        /// </summary>
        public static Color ToRgb(Hsv hsv)
        {
            float v = hsv.V;
            float s = hsv.S;
            float r = v;
            float g = v;
            float b = v;
            if (s > 0)
            {
                float h = hsv.H * 6;
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
            return Color .FromArgb((int)(r * 255), (int)(g * 255), (int)(b * 255));
        }

        public static Hsv black { get { return new Hsv(0, 0, 0); } }
        public static Hsv blue { get { return new Hsv(4f / 6f, 1, 1); } }
        public static Hsv cyan { get { return new Hsv(3f / 6f, 1, 1); } }
        public static Hsv gray { get { return new Hsv(0, 0, 0.5f); } }
        public static Hsv green { get { return new Hsv(2f / 6f, 1, 1); } }
        /// <summary>
        /// English spelling for gray.
        /// </summary>
        public static Hsv grey { get { return gray; } }
        public static Hsv magenta { get { return new Hsv(5f / 6f, 1, 1); } }
        public static Hsv red { get { return new Hsv(0, 1, 1); } }
        public static Hsv white { get { return new Hsv(0, 0, 1); } }
        /// <summary>
        /// UnityEngine.Color.yellow の値が微妙なので、どうしても誤差が発生する。
        /// </summary>
        public static Hsv yellow { get { return new Hsv(0.1533865f, 0.9843137f, 1); } }


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
    }
}