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