//#define ENABLE_SUM_COLOR
using MessagePack;
using MosaicArt.Colors;
using MosaicArt.Images;
using System.Drawing;
using System.Text;
using static MosaicArt.Utility;

namespace MosaicArt
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// ひとつの画像の情報
    /// </summary>
    [MessagePackObject(true)]
    public class ImageInfo
    {
        /// <summary>
        /// 圧縮した画像の幅
        /// </summary>
        public const int MiniImageWidth = 8;
        /// <summary>
        /// 圧縮した画像の高さ
        /// </summary>
        public const int MiniImageHeight = 8;

        /// <summary>
        /// ファイルとして保存されている場合のパス。
        /// アプリケーション内で生成された画像の場合は空文字列;
        /// </summary>
        public string Path = string.Empty;
        public int Width = 0;
        public int Height = 0;
#if ENABLE_SUM_COLOR
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
#endif
        /// <summary>
        /// 圧縮した画像
        /// </summary>
        public Rgb888Image MiniImage = new();
        /// <summary>
        /// 元画像の一時保存用
        /// </summary>
        [IgnoreMember]
        public Bitmap? Bitmap = null;
        /// <summary>
        /// 比較用の画像
        /// </summary>
        [IgnoreMember]
        public Rgb888Image? ComparisonImage = null;
        /// <summary>
        /// 予約済み
        /// </summary>
        [IgnoreMember]
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
            MiniImage = new(bitmap, MiniImageWidth, MiniImageHeight);
        }
        public ImageInfo(string path, Bitmap bitmap)
        {
            Path = path;
            Bitmap = bitmap;
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
            MiniImage = new(bitmap, MiniImageWidth, MiniImageHeight);
        }
#pragma warning disable CA1822 // メンバーを static に設定します
        private void Analyze(Bitmap bitmap)
        {
#if ENABLE_SUM_COLOR
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
#endif
        }
#pragma warning restore CA1822 // メンバーを static に設定します
        /// <summary>
        /// 比較し結果を実数で返す。
        /// 0に近いほど近い情報。
        /// </summary>
        public double Compare(ImageInfo other)
        {
            // 比較用画像生成
            if (ComparisonImage == null)
            {
                MakeComparisonImage(other.Width, other.Height);
            }
            if (other.ComparisonImage == null)
            {
                other.MakeComparisonImage();
            }
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            return SquaredDistance(ComparisonImage, other.ComparisonImage);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
        }
        /// <summary>
        /// 速い比較。そのかわりに厳密ではない。
        /// </summary>
        public double FastCompare(ImageInfo other)
        {
            return SquaredDistance(MiniImage, other.MiniImage);
        }
        /// <summary>
        /// Bitmapがなければ読み込む
        /// </summary>
        public void LoadBitmapIfNull()
        {
            if (Bitmap == null)
            {
                Bitmap = new(Path);
            }
        }
        /// <summary>
        /// 比較用画像を作成
        /// </summary>
        /// <returns>true:成功　false:元画像が無くて失敗</returns>
        public bool MakeComparisonImage()
        {
            if (Bitmap == null)
            {
                if (string.IsNullOrEmpty(Path))
                {
                    return false;
                }
                Bitmap = new(Path);
            }
            if (ComparisonImage == null)
            {
                // まだ作ってない→作成
                ComparisonImage = new(Bitmap);
            }
            else if (Bitmap.Width == ComparisonImage.Width && Bitmap.Height == ComparisonImage.Width)
            {
                // 作成不要
            }
            else
            {
                // サイズがあってない→作成
                ComparisonImage = new(Bitmap);
            }
            return true;
        }
        /// <summary>
        /// 比較用画像を作成
        /// </summary>
        /// <param name="width">比較相手画像の幅</param>
        /// <param name="height">比較相手画像の高さ</param>
        /// <returns>true:成功　false:元画像が無くて失敗</returns>
        public bool MakeComparisonImage(int width, int height)
        {
            lock (this)
            {
                if (Bitmap == null)
                {
                    if (string.IsNullOrEmpty(Path))
                    {
                        return false;
                    }
                    Bitmap = new(Path);
                }
                if (Bitmap.Width == width && Bitmap.Height == height)
                {
                    ComparisonImage = new(Bitmap);
                }
                else
                {
                    ComparisonImage = new(Bitmap, width, height);
                }
            }
            return true;
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}