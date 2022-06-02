using MosaicArt.Core;
using System.Drawing;
using System.Drawing.Imaging;
using static MosaicArt.Core.Utility;

namespace MosaicArt.DummyMaker
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    static class Program
    {
        static void Main(string[] args)
        {
            var directory = @"D:\Develop\Projects\MosaicArt\TestData\Resource\Dummy3";
            MonochromeImage4x4 image = new MonochromeImage4x4();
            Argb2222 zeroColor = new();
            Argb2222 oneColor = new();
            //for (int i = 0; i <= ushort.MaxValue; i++)
            var imagePatterns = new ushort[]
            {
                0b0000_0000_0000_1111,
                0b0000_0000_1111_0000,
                0b0000_1111_0000_0000,
                0b1111_0000_0000_0000,

                0b0001_0001_0001_0001,
                0b0010_0010_0010_0010,
                0b0100_0100_0100_0100,
                0b1000_1000_1000_1000,

                0b0000_0000_1111_1111,
                0b0000_1111_1111_0000,

                0b0011_0011_0011_0011,
                0b0110_0110_0110_0110,

                0b1111_1110_1100_1000,
                0b1111_0111_0011_0001,

                0b1000_1100_1110_1111,
                0b0001_0011_0111_1111,

                0b1110_1100_1000_0000,
                0b0111_0011_0001_0000,

                0b0000_1000_1100_1110,
                0b0000_0001_0011_0111,

                0b1100_1000_0000_0000,
                0b0011_0001_0000_0000,

                0b0000_0000_1000_1100,
                0b0000_0000_0001_0011,

                0b1100_1110_0111_0011,
                0b0011_0111_1110_1100,
            };
            //for (int i = 0; i <= 0xFFFF; i++)
            for (int i = 0; i < imagePatterns.Length; i++)
            {
                //image.Bits = (ushort)i;
                image.Bits = imagePatterns[i];
                var directory2 = directory + $"/{image.Bits.ToString("X4")}";
                if(Directory.Exists(directory2))
                {
                    continue;// すでにあるならスキップ
                }
                Directory.CreateDirectory(directory2);

                for (int j = 0b11000000; j <= 0b11111111; j++)
                {
                    zeroColor.Bits = (byte)j;
                    for (int k = 0b11000000; k <= 0b11111111; k++)
                    {
                        if (k == j) { continue; }
                        oneColor.Bits = (byte)k;

                        var path = directory2 + $"/{j.ToString("X2")}_{k.ToString("X2")}.bmp";
                        Console.WriteLine(path);
                        if (File.Exists(path))
                        {
                            continue;
                        }
                        var bitmap = image.ToBitmap(zeroColor, oneColor);
                        bitmap.Save(path, ImageFormat.Bmp);
                    }
                }
            }
        }

        static void Dummy2()
        {
            var directory = @"D:\Develop\Projects\MosaicArt\TestData\Resource\Dummy2";
            Bitmap bitmap = new Bitmap(1, 1);
            uint argb;
            uint a = 0xFF000000;
            for (uint r = 0; r <= 0xFF0000; r += 0x110000)
            {
                for (uint g = 0; g <= 0xFF00; g += 0x1100)
                {
                    for (uint b = 0; b <= 0xFF; b += 0x11)
                    {
                        argb = a | r | g | b;
                        bitmap.SetPixel(0, 0, ColorFromArgb(argb));
                        bitmap.Save($@"{directory}/{argb.ToString("X8")}.bmp", ImageFormat.Bmp);
                    }
                }
            }
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}