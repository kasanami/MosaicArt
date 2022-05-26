using System.Drawing;

namespace MosaicArt.ImageAnalyzer;

public class Program
{
#pragma warning disable CA1416 // プラットフォームの互換性を検証
    public static void Main()
    {
        var file = @"D:\Develop\Projects\MosaicArt\TestData\Target1";
        {
            var bitmap = new Bitmap(file);
            var imageInfo = new ImageInfo(bitmap);
        }
    }
#pragma warning restore CA1416 // プラットフォームの互換性を検証
}