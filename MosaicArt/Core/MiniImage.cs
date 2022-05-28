using MessagePack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicArt.Core
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// 圧縮した画像
    /// </summary>
    [MessagePackObject(true)]
    public class MiniImage
    {
        public int Width = 0;
        public int Height = 0;
        public List<byte> Pixels = new();
        public MiniImage()
        {
        }
        public MiniImage(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var color = bitmap.GetPixel(x, y);
                    Pixels.Add((Rgb332)color);
                }
            }
        }
        public MiniImage(Bitmap bitmap, int width, int height)
        {
            Bitmap bitmap2 = bitmap.Resize(width, height);
            Width = width;
            Height = height;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    var color = bitmap2.GetPixel(x, y);
                    Pixels.Add((Rgb332)color);
                }
            }
        }
        public Rgb332 GetPixel(int x, int y)
        {
            return (Rgb332)Pixels[(y * Width) + x];
        }
        public void SetPixel(int x, int y, Rgb332 rgb)
        {
            Pixels[(y * Width) + x] = rgb;
        }
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

        public void Save(string path, ImageFormat imageFormat)
        {
            var bitmap= ToBitmap();
            bitmap.Save(path, imageFormat);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}