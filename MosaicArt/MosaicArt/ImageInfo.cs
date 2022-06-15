﻿//#define ENABLE_SUM_COLOR
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
        public Bitmap? BitmapForComparison = null;
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
            // サイズを合わせた比較用画像
            if (BitmapForComparison == null)
            {
                BitmapForComparison = new Bitmap(bitmap0, bitmap1.Width, bitmap1.Height);
            }
            else if (BitmapForComparison.Width != bitmap1.Width || BitmapForComparison.Height != bitmap1.Height)
            {
                BitmapForComparison = new Bitmap(bitmap0, bitmap1.Width, bitmap1.Height);
            }
            return SquaredDistance(BitmapForComparison, bitmap1);
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
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}