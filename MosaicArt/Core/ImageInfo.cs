using MessagePack;
using System.Drawing;
using System.Text;

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
        public Rgb SumRgb = new();
        /// <summary>
        /// 画像全体の平均色
        /// </summary>
        public Rgb AverageRgb = new();
        /// <summary>
        /// 画像全体の平均色
        /// </summary>
        public Hsv AverageHsv = new();
        /// <summary>
        /// 圧縮した画像
        /// </summary>
        public MonochromeImage4x4 MiniImage = new();
        public ImageInfo()
        {
        }
        public ImageInfo(Bitmap bitmap)
        {
            Path = string.Empty;
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
            MiniImage = new(bitmap);
        }
        public ImageInfo(string path, Bitmap bitmap)
        {
            Path = path;
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
            MiniImage = new(bitmap);
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
            // 1ピクセルの重みに注意（1ピクセル=1）
            double sum = 0;
            //sum += Distance(MiniImage, other.MiniImage);
            sum += Utility.Distance(AverageRgb, other.AverageRgb);
            sum += MonochromeImage4x4.PixelCount - MiniImage.MatchCount(other.MiniImage);// 全一致なら0となる
            return sum;
        }
        /// <summary>
        /// 色空間内での距離
        /// </summary
        public static double Distance(MiniImage image0, MiniImage image1)
        {
            double sum = 0;
            for (int i = 0; i < image0.Bytes.Count; i++)
            {
                var diff = image0.Bytes[i] - image1.Bytes[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}