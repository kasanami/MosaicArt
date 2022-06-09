using System.Drawing;

namespace MosaicArt.Core
{
    /// <summary>
    /// Colorの拡張関数
    /// </summary>
    public static class ColorExtensions
    {
        /// <summary>
        /// 輝度(0～255の値)
        /// </summary>
        public static byte GetLuminance(this Color color)
        {
            return (byte)Math.Round(color.R * 0.298912 + color.G * 0.586611 + color.B * 0.114478);
        }
    }
}