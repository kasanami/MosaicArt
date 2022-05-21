using Microsoft.VisualStudio.TestTools.UnitTesting;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Tests
{
    [TestClass()]
    public class HsvTests
    {
        [TestMethod()]
        public void FromRgbTest()
        {
            // 赤
            Hsv hsv = Hsv.FromRgb(1, 0, 0);
            Assert.AreEqual(Hsv.Red, hsv);
            // 緑
            hsv = Hsv.FromRgb(0, 1, 0);
            Assert.AreEqual(Hsv.Green, hsv);
            // 青
            hsv = Hsv.FromRgb(0, 0, 1);
            Assert.AreEqual(Hsv.Blue, hsv);
            // 白
            hsv = Hsv.FromRgb(1, 1, 1);
            Assert.AreEqual(Hsv.White, hsv);
            // 黒
            hsv = Hsv.FromRgb(0, 0, 0);
            Assert.AreEqual(Hsv.Black, hsv);
        }
        [TestMethod()]
        public void ToRgbTest()
        {
            // 赤
            Rgb rgb = Hsv.ToRgb(Hsv.Red);
            Assert.AreEqual(Rgb.Red, rgb);
            // 緑
            rgb = Hsv.ToRgb(Hsv.Green);
            Assert.AreEqual(Rgb.Green, rgb);
            // 青
            rgb = Hsv.ToRgb(Hsv.Blue);
            Assert.AreEqual(Rgb.Blue, rgb);
            // 白
            rgb = Hsv.ToRgb(Hsv.White);
            Assert.AreEqual(Rgb.White, rgb);
            // 黒
            rgb = Hsv.ToRgb(Hsv.Black);
            Assert.AreEqual(Rgb.Black, rgb);
        }
    }
}