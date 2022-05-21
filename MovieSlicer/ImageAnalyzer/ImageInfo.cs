﻿using Core;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageAnalyzer
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    /// <summary>
    /// ひとつの画像の情報
    /// </summary>
    [MessagePackObject(true)]
    public class ImageInfo
    {
        int Width;
        int Height;
        /// <summary>
        /// 画像全体の平均色
        /// </summary>
        Rgb AverageRgb = new Rgb();
        /// <summary>
        /// 画像全体の平均色
        /// </summary>
        Hsv AverageHsv = new Hsv();
        public ImageInfo(Bitmap bitmap)
        {
            Width = bitmap.Width;
            Height = bitmap.Height;
            Analyze(bitmap);
        }
        private void Analyze(Bitmap bitmap)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    AverageRgb += pixel;
                }
            }
            var area = Width * Height;
            if (area > 0)
            {
                AverageRgb /= area;
            }
            AverageHsv = (Hsv)AverageRgb;
        }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
    }
}