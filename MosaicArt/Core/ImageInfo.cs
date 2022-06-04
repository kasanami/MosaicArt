using MessagePack;
using System.Drawing;
using System.Text;
using static MosaicArt.Core.Utility;

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
        /// <summary>
        /// 
        /// </summary>
        [IgnoreMember]
        public Bitmap? Bitmap = null;
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
        public MiniImage4x4 MiniImage = new();
        /// <summary>
        /// 予約済み
        /// </summary>
        public bool IsReserved = false;

        public ImageInfo()
        {
        }
        public ImageInfo(Bitmap bitmap)
        {
            Path = string.Empty;
            Bitmap = bitmap;
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
            MiniImage = new(bitmap);
        }
        public ImageInfo(string path, Bitmap bitmap)
        {
            Path = path;
            Bitmap = bitmap;
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
            Bitmap? bitmap0 = null;
            Bitmap? bitmap1 = null;
            if (Bitmap != null)
            {
                bitmap0 = Bitmap;
            }
            else if (string.IsNullOrEmpty(Path) == false)
            {
                Bitmap = new(Path);
                bitmap0 = Bitmap;
            }
            if (other.Bitmap != null)
            {
                bitmap1 = other.Bitmap;
            }
            else if (string.IsNullOrEmpty(other.Path) == false)
            {
                other.Bitmap = new(other.Path);
                bitmap1 = other.Bitmap;
            }
            if (bitmap0 == null || bitmap1 == null)
            {
                return double.PositiveInfinity;
            }
            return Distance(bitmap0, bitmap1);
        }
        public double PrimaryCompare(ImageInfo other)
        {
            return Distance(AverageRgb, other.AverageRgb);
        }
        public double SecondaryCompare(ImageInfo other)
        {
            return Distance(MiniImage, other.MiniImage);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}