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
    public class MonochromeImage4x4Tests
    {
        [TestMethod()]
        public void SetPixelTest()
        {
            MonochromeImage4x4 monochrome = new MonochromeImage4x4();
            Assert.AreEqual(0b0000_0000_0000_0000, monochrome.Bits);
            monochrome.SetPixel(0, 0, true);
            Assert.AreEqual(0b0000_0000_0000_0001, monochrome.Bits);
            monochrome.SetPixel(1, 0, true);
            Assert.AreEqual(0b0000_0000_0000_0011, monochrome.Bits);
            monochrome.SetPixel(2, 0, true);
            Assert.AreEqual(0b0000_0000_0000_0111, monochrome.Bits);
            monochrome.SetPixel(3, 0, true);
            Assert.AreEqual(0b0000_0000_0000_1111, monochrome.Bits);
            monochrome.SetPixel(0, 1, true);
            Assert.AreEqual(0b0000_0000_0001_1111, monochrome.Bits);
            monochrome.SetPixel(1, 1, true);
            Assert.AreEqual(0b0000_0000_0011_1111, monochrome.Bits);
            monochrome.SetPixel(2, 1, true);
            Assert.AreEqual(0b0000_0000_0111_1111, monochrome.Bits);
            monochrome.SetPixel(3, 1, true);
            Assert.AreEqual(0b0000_0000_1111_1111, monochrome.Bits);
            monochrome.SetPixel(0, 2, true);
            Assert.AreEqual(0b0000_0001_1111_1111, monochrome.Bits);
            monochrome.SetPixel(1, 2, true);
            Assert.AreEqual(0b0000_0011_1111_1111, monochrome.Bits);
            monochrome.SetPixel(2, 2, true);
            Assert.AreEqual(0b0000_0111_1111_1111, monochrome.Bits);
            monochrome.SetPixel(3, 2, true);
            Assert.AreEqual(0b0000_1111_1111_1111, monochrome.Bits);
            monochrome.SetPixel(0, 3, true);
            Assert.AreEqual(0b0001_1111_1111_1111, monochrome.Bits);
            monochrome.SetPixel(1, 3, true);
            Assert.AreEqual(0b0011_1111_1111_1111, monochrome.Bits);
            monochrome.SetPixel(2, 3, true);
            Assert.AreEqual(0b0111_1111_1111_1111, monochrome.Bits);
            monochrome.SetPixel(3, 3, true);
            Assert.AreEqual(0b1111_1111_1111_1111, monochrome.Bits);

            monochrome.SetPixel(0, 0, false);
            Assert.AreEqual(0b1111_1111_1111_1110, monochrome.Bits);
            monochrome.SetPixel(1, 0, false);
            Assert.AreEqual(0b1111_1111_1111_1100, monochrome.Bits);
            monochrome.SetPixel(2, 0, false);
            Assert.AreEqual(0b1111_1111_1111_1000, monochrome.Bits);
            monochrome.SetPixel(3, 0, false);
            Assert.AreEqual(0b1111_1111_1111_0000, monochrome.Bits);
            monochrome.SetPixel(0, 1, false);
            Assert.AreEqual(0b1111_1111_1110_0000, monochrome.Bits);
            monochrome.SetPixel(1, 1, false);
            Assert.AreEqual(0b1111_1111_1100_0000, monochrome.Bits);
            monochrome.SetPixel(2, 1, false);
            Assert.AreEqual(0b1111_1111_1000_0000, monochrome.Bits);
            monochrome.SetPixel(3, 1, false);
            Assert.AreEqual(0b1111_1111_0000_0000, monochrome.Bits);
            monochrome.SetPixel(0, 2, false);
            Assert.AreEqual(0b1111_1110_0000_0000, monochrome.Bits);
            monochrome.SetPixel(1, 2, false);
            Assert.AreEqual(0b1111_1100_0000_0000, monochrome.Bits);
            monochrome.SetPixel(2, 2, false);
            Assert.AreEqual(0b1111_1000_0000_0000, monochrome.Bits);
            monochrome.SetPixel(3, 2, false);
            Assert.AreEqual(0b1111_0000_0000_0000, monochrome.Bits);
            monochrome.SetPixel(0, 3, false);
            Assert.AreEqual(0b1110_0000_0000_0000, monochrome.Bits);
            monochrome.SetPixel(1, 3, false);
            Assert.AreEqual(0b1100_0000_0000_0000, monochrome.Bits);
            monochrome.SetPixel(2, 3, false);
            Assert.AreEqual(0b1000_0000_0000_0000, monochrome.Bits);
            monochrome.SetPixel(3, 3, false);
            Assert.AreEqual(0b0000_0000_0000_0000, monochrome.Bits);
        }
    }
}