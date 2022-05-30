using System.Drawing;
using System.Drawing.Imaging;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 画像の抽象クラス
    /// </summary>
    public abstract class AbstractImage
    {
        /// <summary>
        /// 画像の幅[ピクセル]
        /// </summary>
        public abstract int Width { get; set; }
        /// <summary>
        /// 画像の高さ[ピクセル]
        /// </summary>
        public abstract int Height { get; set; }
        /// <summary>
        /// ピクセルの色を取得
        /// </summary>
        public abstract Color GetPixel(int x, int y);
        /// <summary>
        /// ピクセルの色を設定
        /// </summary>
        public abstract void SetPixel(int x, int y, Color color);
        /// <summary>
        /// Bitmap に変換する。
        /// </summary>
        public Bitmap ToBitmap()
        {
            var bitmap = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color color = GetPixel(x, y);
                    bitmap.SetPixel(x, y, color);
                }
            }
            return bitmap;
        }
        /// <summary>
        /// ファイルに保存する。
        /// </summary>
        public void Save(string path, ImageFormat imageFormat)
        {
            var bitmap = ToBitmap();
            bitmap.Save(path, imageFormat);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}