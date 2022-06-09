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
        /// 画像の面積[ピクセル]
        /// </summary>
        public long Area { get { return Width + Height; } }
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
        /// <summary>
        /// ２つの画像を比較します。
        /// 全一致なら0を返します。最大差異は1とする。
        /// </summary>
        public double Compare(AbstractImage other)
        {
            if (other.Width != Width || other.Height != Height)
            {
                throw new ArgumentException($"画像のサイズが一致していません。this={Width}x{Height} {nameof(other)}={other.Width}x{other.Height}");
            }
            double sum = 0;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color color0 = GetPixel(x, y);
                    Color color1 = other.GetPixel(x, y);
                    sum += Utility.Distance(color0, color1) / 510;// /510で最大値を1にする。
                }
            }
            return sum / Area;
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}