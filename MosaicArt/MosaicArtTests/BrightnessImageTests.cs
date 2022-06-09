using Microsoft.VisualStudio.TestTools.UnitTesting;
using MosaicArt.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MosaicArt.Core.Tests
{
    [TestClass()]
    public class BrightnessImageTests
    {
        [TestMethod()]
        public void ToBrightnessTest()
        {
            var brightness = BrightnessImage.ToBrightness(Color.FromArgb(3, 2, 1));
            Assert.AreEqual(3, brightness);

            brightness = BrightnessImage.ToBrightness(Color.FromArgb(1, 2, 3));
            Assert.AreEqual(3, brightness);

            brightness = BrightnessImage.ToBrightness(Color.FromArgb(1, 3, 2));
            Assert.AreEqual(3, brightness);

            brightness = BrightnessImage.ToBrightness(Color.FromArgb(2, 3, 1));
            Assert.AreEqual(3, brightness);
        }
    }
}