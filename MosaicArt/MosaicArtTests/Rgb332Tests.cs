using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;

namespace MosaicArt.Colors.Tests
{
    [TestClass()]
    public class Rgb332Tests
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            Rgb332 rgb = new Rgb332(Rgb.Red);
            Assert.AreEqual(Rgb332.Red, rgb);
            rgb = new Rgb332(Rgb.Green);
            Assert.AreEqual(Rgb332.Green, rgb);
            rgb = new Rgb332(Rgb.Blue);
            Assert.AreEqual(Rgb332.Blue, rgb);
            rgb = new Rgb332(Rgb.White);
            Assert.AreEqual(Rgb332.White, rgb);
            rgb = new Rgb332(Rgb.Black);
            Assert.AreEqual(Rgb332.Black, rgb);

            rgb = new Rgb332(1, 0, 0);
            Assert.AreEqual(0b0010_0000, rgb.Bits);
            rgb = new Rgb332(0, 1, 0);
            Assert.AreEqual(0b0000_0100, rgb.Bits);
            rgb = new Rgb332(0, 0, 1);
            Assert.AreEqual(0b0000_0001, rgb.Bits);
            rgb = new Rgb332(0, 0, 0);
            Assert.AreEqual(0b0000_0000, rgb.Bits);
        }
        [TestMethod()]
        public void PropertiesTest()
        {
            Rgb332 rgb = Rgb332.White;
            for (int i = 0; i <= Rgb332.RMax; i++)
            {
                rgb.R = i;
                Assert.AreEqual(i, rgb.R);
                Assert.AreEqual(Rgb332.GMax, rgb.G);
                Assert.AreEqual(Rgb332.BMax, rgb.B);
            }
            for (int i = 0; i <= Rgb332.GMax; i++)
            {
                rgb.G = i;
                Assert.AreEqual(i, rgb.G);
                Assert.AreEqual(Rgb332.RMax, rgb.R);
                Assert.AreEqual(Rgb332.BMax, rgb.B);
            }
            for (int i = 0; i <= Rgb332.BMax; i++)
            {
                rgb.B = i;
                Assert.AreEqual(i, rgb.B);
                Assert.AreEqual(Rgb332.RMax, rgb.R);
                Assert.AreEqual(Rgb332.GMax, rgb.G);
            }

            rgb = Rgb332.Zero;
            for (int i = 0; i <= Rgb332.RMax; i++)
            {
                rgb.R = i;
                Assert.AreEqual(i, rgb.R);
                Assert.AreEqual(0, rgb.G);
                Assert.AreEqual(0, rgb.B);
            }
            rgb = Rgb332.Zero;
            for (int i = 0; i <= Rgb332.GMax; i++)
            {
                rgb.G = i;
                Assert.AreEqual(0, rgb.R);
                Assert.AreEqual(i, rgb.G);
                Assert.AreEqual(0, rgb.B);
            }
            rgb = Rgb332.Zero;
            for (int i = 0; i <= Rgb332.BMax; i++)
            {
                rgb.B = i;
                Assert.AreEqual(0, rgb.R);
                Assert.AreEqual(0, rgb.G);
                Assert.AreEqual(i, rgb.B);
            }
        }

        [TestMethod()]
        public void OperatorsTest()
        {
            for (int i = 0; i <= byte.MaxValue; i++)
            {
                Rgb332 rgb = (byte)i;
                byte bits = (byte)rgb;
                Assert.AreEqual(i, bits);
            }

            {
                Rgb332 rgb = new Rgb332(1,1,1);
                Color color = (Color)rgb;
                Assert.AreEqual(0b001_001_00, color.R);
                Assert.AreEqual(0b001_001_00, color.G);
                Assert.AreEqual(0b01_01_01_01, color.B);
            }

            {
                Rgb332 rgb = new Rgb332(2, 2, 2);
                Color color = (Color)rgb;
                Assert.AreEqual(0b010_010_01, color.R);
                Assert.AreEqual(0b010_010_01, color.G);
                Assert.AreEqual(0b10_10_10_10, color.B);
            }

            {
                Rgb332 rgb = new Rgb332(3, 3, 3);
                Color color = (Color)rgb;
                Assert.AreEqual(0b011_011_01, color.R);
                Assert.AreEqual(0b011_011_01, color.G);
                Assert.AreEqual(0b11_11_11_11, color.B);
            }

            {
                Color color0 = Color.Red;
                Rgb332 rgb = (Rgb332)color0;
                Color color1 = (Color)rgb;
                Assert.AreEqual(color0.R, color1.R);
                Assert.AreEqual(color0.G, color1.G);
                Assert.AreEqual(color0.B, color1.B);
            }

            {
                Color color0 = Color.FromArgb(0, 255, 0);
                Rgb332 rgb = (Rgb332)color0;
                Color color1 = (Color)rgb;
                Assert.AreEqual(color0.R, color1.R);
                Assert.AreEqual(color0.G, color1.G);
                Assert.AreEqual(color0.B, color1.B);
            }

            {
                Color color0 = Color.Blue;
                Rgb332 rgb = (Rgb332)color0;
                Color color1 = (Color)rgb;
                Assert.AreEqual(color0.R, color1.R);
                Assert.AreEqual(color0.G, color1.G);
                Assert.AreEqual(color0.B, color1.B);
            }

            {
                Color color0 = Color.White;
                Rgb332 rgb = (Rgb332)color0;
                Color color1 = (Color)rgb;
                Assert.AreEqual(color0.R, color1.R);
                Assert.AreEqual(color0.G, color1.G);
                Assert.AreEqual(color0.B, color1.B);
            }

            {
                Color color0 = Color.Black;
                Rgb332 rgb = (Rgb332)color0;
                Color color1 = (Color)rgb;
                Assert.AreEqual(color0.R, color1.R);
                Assert.AreEqual(color0.G, color1.G);
                Assert.AreEqual(color0.B, color1.B);
            }
        }
    }
}