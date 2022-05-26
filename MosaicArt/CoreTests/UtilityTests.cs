using Microsoft.VisualStudio.TestTools.UnitTesting;
using MosaicArt.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MosaicArt.Core.Tests
{
    [TestClass()]
    public class UtilityTests
    {
        [TestMethod()]
        public void DistanceTest()
        {
            Color color0 = Color.Red;
            Color color1 = Color.Red;
            Assert.AreEqual(0.0, Utility.Distance(color0, color1));
            color0 = Color.Green;
            color1 = Color.Green;
            Assert.AreEqual(0.0, Utility.Distance(color0, color1));
            color0 = Color.Blue;
            color1 = Color.Blue;
            Assert.AreEqual(0.0, Utility.Distance(color0, color1));
            color0 = Color.Black;
            color1 = Color.Red;
            Assert.AreEqual(255.0, Utility.Distance(color0, color1));
            color0 = Color.Black;
            color1 = Color.White;
            Assert.AreEqual(441.672955930063709849498817084, Utility.Distance(color0, color1));
        }
    }
}