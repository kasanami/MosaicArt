using Microsoft.VisualStudio.TestTools.UnitTesting;
using MosaicArt.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicArt.Core.Tests
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
        }
    }
}