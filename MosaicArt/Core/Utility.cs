using System.Drawing;

namespace MosaicArt.Core
{
    public static class Utility
    {
        /// <summary>
        /// 色空間内での距離
        /// ※Color の各値の範囲は 0～255 なので注意
        /// </summary
        public static double Distance(Color color0, Color color1)
        {
            var r = color0.R - color1.R;
            var g = color0.G - color1.G;
            var b = color0.B - color1.B;
            var a = color0.A - color1.A;
            return Math.Sqrt(r * r + g * g + b * b + a * a);
        }
        /// <summary>
        /// 色空間内での距離
        /// </summary
        public static double Distance(Rgb color0, Rgb color1)
        {
            var r = color0.R - color1.R;
            var g = color0.G - color1.G;
            var b = color0.B - color1.B;
            return Math.Sqrt(r * r + g * g + b * b);
        }
        /// <summary>
        /// 符号なし32ビット整数からColorを生成する。
        /// </summary>
        public static Color ColorFromArgb(uint argb)
        {
            return Color.FromArgb((int)argb);
        }
    }
}