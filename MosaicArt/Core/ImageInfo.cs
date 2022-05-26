using MosaicArt.Core;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// ひとつの画像の情報
    /// </summary>
    [MessagePackObject(true)]
    public class ImageInfo
    {
        /// <summary>
        /// ファイルとして保存されている場合のパス。
        /// アプリケーション内で生成された画像の場合は空文字列;
        /// </summary>
        public string Path = string.Empty;
        public int Width = 0;
        public int Height = 0;
        /// <summary>
        /// 画像全体の合計色
        /// </summary>
        public Rgb SumRgb = new Rgb();
        /// <summary>
        /// 画像全体の平均色
        /// </summary>
        public Rgb AverageRgb = new Rgb();
        /// <summary>
        /// 画像全体の平均色
        /// </summary>
        public Hsv AverageHsv = new Hsv();
        public ImageInfo()
        {
        }
        public ImageInfo(Bitmap bitmap)
        {
            Path = string.Empty;
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
        }
        public ImageInfo(string path, Bitmap bitmap)
        {
            Path = path;
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
        }
        private void Analyze(Bitmap bitmap)
        {
            SumRgb = Rgb.Zero;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    SumRgb += pixel;
                }
            }
            var area = Width * Height;
            if (area > 0)
            {
                AverageRgb = SumRgb / area;
            }
            AverageHsv = (Hsv)AverageRgb;
        }
        /// <summary>
        /// 比較し結果を実数で返す。
        /// 0に近いほど近い情報。
        /// </summary>
        public double Compare(ImageInfo other)
        {
            return Utility.Distance(AverageRgb, other.AverageRgb);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}