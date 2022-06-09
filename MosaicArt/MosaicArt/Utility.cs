using MosaicArt.Colors;
using MosaicArt.Images;
using System.Drawing;

namespace MosaicArt
{
    public static class Utility
    {
        #region Color
        /// <summary>
        /// 色空間内での距離
        /// ※Color の各値の範囲は 0～255 なので注意
        /// NOTE:最大値、α値も含めた場合　510 となる。
        /// α値を除くと、441.672955930063709849498817084となる。
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
        /// 色空間内での距離
        /// </summary
        public static double Distance(Rgb332 color0, Rgb332 color1)
        {
            var r = color0.R - color1.R;
            var g = color0.G - color1.G;
            var b = color0.B - color1.B;
            return Math.Sqrt(r * r + g * g + b * b);
        }
        /// <summary>
        /// 色空間内での距離
        /// </summary
        public static double Distance(Hsv color0, Hsv color1)
        {
            var h = color0.H - color1.H;
            var s = color0.S - color1.S;
            var v = color0.V - color1.V;
            return Math.Sqrt(h * h + s * s + v * v);
        }
        /// <summary>
        /// 符号なし32ビット整数からColorを生成する。
        /// </summary>
        public static Color ColorFromArgb(uint argb)
        {
            return Color.FromArgb((int)argb);
        }
        #endregion Color

        #region Image
        /// <summary>
        /// 色空間内での距離
        /// </summary
        public static double Distance(Bitmap image0, Bitmap image1)
        {
            double sum = 0;
#pragma warning disable CA1416 // プラットフォームの互換性を検証
            lock (image0)
            {
                lock (image1)
                {
                    if (image0.Width != image1.Width || image0.Height != image1.Height)
                    {
                        image1 = image1.Resize(image0.Width, image0.Height);
                    }
                    for (int y = 0; y < image0.Height; y++)
                    {
                        for (int x = 0; x < image0.Width; x++)
                        {
                            var color0 = image0.GetPixel(x, y);
                            var color1 = image1.GetPixel(x, y);
                            sum += Distance(color0, color1);
                        }
                    }
                }
            }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
            return sum;
        }
        /// <summary>
        /// 色空間内での距離
        /// </summary
        public static double Distance(MiniImage image0, MiniImage image1)
        {
            double squaredDistance = SquaredDistance(image0, image1);
            return Math.Sqrt(squaredDistance);
        }
        /// <summary>
        /// 色空間内での距離
        /// </summary
        public static double SquaredDistance(MiniImage image0, MiniImage image1)
        {
            double sum = 0;
            for (int i = 0; i < image0.Bytes.Count; i++)
            {
                var diff = image0.Bytes[i] - image1.Bytes[i];
                sum += diff * diff;
            }
            return sum;
        }
        #endregion Image

        #region ビット操作
        /// <summary>
        /// ビット数に対する最大値 (添字にビット数を入れる)
        /// </summary>
        public static readonly ulong[] BitsToMaxValue = new ulong[65]
        {
            0,
            0x1,
            0x3,
            0x7,
            0xF,
            0x1F,
            0x3F,
            0x7F,
            0xFF,
            0x1FF,
            0x3FF,
            0x7FF,
            0xFFF,
            0x1FFF,
            0x3FFF,
            0x7FFF,
            0xFFFF,
            0x1_FFFF,
            0x3_FFFF,
            0x7_FFFF,
            0xF_FFFF,
            0x1F_FFFF,
            0x3F_FFFF,
            0x7F_FFFF,
            0xFF_FFFF,
            0x1FF_FFFF,
            0x3FF_FFFF,
            0x7FF_FFFF,
            0xFFF_FFFF,
            0x1FFF_FFFF,
            0x3FFF_FFFF,
            0x7FFF_FFFF,
            0xFFFF_FFFF,
            0x1_FFFF_FFFF,
            0x3_FFFF_FFFF,
            0x7_FFFF_FFFF,
            0xF_FFFF_FFFF,
            0x1F_FFFF_FFFF,
            0x3F_FFFF_FFFF,
            0x7F_FFFF_FFFF,
            0xFF_FFFF_FFFF,
            0x1FF_FFFF_FFFF,
            0x3FF_FFFF_FFFF,
            0x7FF_FFFF_FFFF,
            0xFFF_FFFF_FFFF,
            0x1FFF_FFFF_FFFF,
            0x3FFF_FFFF_FFFF,
            0x7FFF_FFFF_FFFF,
            0xFFFF_FFFF_FFFF,
            0x1_FFFF_FFFF_FFFF,
            0x3_FFFF_FFFF_FFFF,
            0x7_FFFF_FFFF_FFFF,
            0xF_FFFF_FFFF_FFFF,
            0x1F_FFFF_FFFF_FFFF,
            0x3F_FFFF_FFFF_FFFF,
            0x7F_FFFF_FFFF_FFFF,
            0xFF_FFFF_FFFF_FFFF,
            0x1FF_FFFF_FFFF_FFFF,
            0x3FF_FFFF_FFFF_FFFF,
            0x7FF_FFFF_FFFF_FFFF,
            0xFFF_FFFF_FFFF_FFFF,
            0x1FFF_FFFF_FFFF_FFFF,
            0x3FFF_FFFF_FFFF_FFFF,
            0x7FFF_FFFF_FFFF_FFFF,
            0xFFFF_FFFF_FFFF_FFFF,
        };

        /// <summary>
        /// 0～最大値の割合を伸縮させるよう形で、ビット数を変更する。
        /// 例：0b111→0b11111111
        /// </summary>
        public static ulong ElasticityBits(ulong value, int fromBits, int toBits)
        {
            var fromMaxValue = BitsToMaxValue[fromBits];
            var toMaxValue = BitsToMaxValue[toBits];
            var rate = (double)value / fromMaxValue;
            return (ulong)Math.Round(rate * toMaxValue);
        }
        /// <summary>
        /// 0～最大値の割合を伸縮させるよう形で、ビット数を変更する。
        /// 例：0b111→0b11111111
        /// </summary>
        public static byte ElasticityBits3To8(byte value)
        {
            return (byte)((value << 5) | (value << 2) | (value >> 1));
        }
        /// <summary>
        /// 変換前の値を添字に入れると、変換後の値が参照される配列。
        /// </summary>
        public static readonly byte[] ElasticityBits8To3Array = new byte[256]
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,6,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,7,
        };
        /// <summary>
        /// 変換前の値を添字に入れると、変換後の値が参照される配列。
        /// </summary>
        public static readonly byte[] ElasticityBits3To8Array = new byte[8]
        {
            0,36,73,109,146,182,219,255,
        };
        /// <summary>
        /// 変換前の値を添字に入れると、変換後の値が参照される配列。
        /// </summary>
        public static readonly byte[] ElasticityBits8To2Array = new byte[256]
        {
            0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,3,
        };
        /// <summary>
        /// 変換前の値を添字に入れると、変換後の値が参照される配列。
        /// </summary>
        public static readonly byte[] ElasticityBits2To8Array = new byte[4]
        {
            0,85,170,255,
        };

        /// <summary>
        ///  1 のビットを数える
        /// </summary>
        public static int CountOne(uint bits)
        {
            bits = (bits & 0x55555555) + (bits >> 1 & 0x55555555);
            bits = (bits & 0x33333333) + (bits >> 2 & 0x33333333);
            bits = (bits & 0x0f0f0f0f) + (bits >> 4 & 0x0f0f0f0f);
            bits = (bits & 0x00ff00ff) + (bits >> 8 & 0x00ff00ff);
            bits = (bits & 0x0000ffff) + (bits >> 16 & 0x0000ffff);
            return (int)bits;
        }
        /// <summary>
        /// 2つのビット配列一致しているビットを数える。
        /// ※1だけではなく0の一致もカウントする。
        /// </summary>
        public static int MatchCount(ushort bits0, ushort bits1)
        {
            ushort bits = (ushort)(bits0 ^ bits1);
            return CountOne((ushort)~bits);
        }
        #endregion ビット操作
        public static void Shuffle<T>(List<T> list, Random random)
        {
            T temp;
            for (int i = 0; i < list.Count; i++)
            {
                var index = random.Next(list.Count);
                temp = list[i];
                list.RemoveAt(i);
                list.Insert(index, temp);
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            Random random = new Random();
            Shuffle(list, random);
        }
    }
}