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
    public class RgbTests
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            Rgb rgb = new Rgb(System.Drawing.Color.Red);
            Assert.AreEqual(Rgb.Red, rgb);
            rgb = new Rgb(System.Drawing.Color.Lime);
            Assert.AreEqual(Rgb.Green, rgb);
            rgb = new Rgb(System.Drawing.Color.Blue);
            Assert.AreEqual(Rgb.Blue, rgb);
            rgb = new Rgb(System.Drawing.Color.White);
            Assert.AreEqual(Rgb.White, rgb);
            rgb = new Rgb(System.Drawing.Color.Black);
            Assert.AreEqual(Rgb.Black, rgb);
            rgb = new Rgb(0, 0, 0);
            Assert.AreEqual(Rgb.Zero, rgb);
        }
        [TestMethod()]
        public void ConstantsTest()
        {
            // 赤
            Rgb rgb = new Rgb(1, 0, 0);
            Assert.AreEqual(Rgb.Red, rgb);
            // 緑
            rgb = new Rgb(0, 1, 0);
            Assert.AreEqual(Rgb.Green, rgb);
            // 青
            rgb = new Rgb(0, 0, 1);
            Assert.AreEqual(Rgb.Blue, rgb);
            // 白
            rgb = new Rgb(1, 1, 1);
            Assert.AreEqual(Rgb.White, rgb);
            // 黒
            rgb = new Rgb(0, 0, 0);
            Assert.AreEqual(Rgb.Black, rgb);
        }
    }
}