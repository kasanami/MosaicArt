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
            color0 = Color.FromArgb(0, 0, 0, 0);
            color1 = Color.White;
            Assert.AreEqual(510, Utility.Distance(color0, color1));
        }
        [TestMethod()]
        public void CountOneTest()
        {
            Assert.AreEqual(0, Utility.CountOne(0b0));
            Assert.AreEqual(1, Utility.CountOne(0b1));
            Assert.AreEqual(1, Utility.CountOne(0b10));

            Assert.AreEqual(2, Utility.CountOne(0b11));
            Assert.AreEqual(2, Utility.CountOne(0b101));

            Assert.AreEqual(3, Utility.CountOne(0b111));
            Assert.AreEqual(3, Utility.CountOne(0b1011));
            Assert.AreEqual(3, Utility.CountOne(0b10101));
        }
        [TestMethod()]
        public void MatchCountTest()
        {
            // 全一致
            Assert.AreEqual(16, Utility.MatchCount(0b0000_0000_0000_0001, 0b0000_0000_0000_0001));
            Assert.AreEqual(16, Utility.MatchCount(0b0000_0000_0000_0010, 0b0000_0000_0000_0010));
            Assert.AreEqual(16, Utility.MatchCount(0b0000_0000_0000_0111, 0b0000_0000_0000_0111));
            Assert.AreEqual(16, Utility.MatchCount(0b0000_0000_0001_0101, 0b0000_0000_0001_0101));

            Assert.AreEqual(0, Utility.MatchCount(0b0000_0000_0000_0000, 0b1111_1111_1111_1111));
            Assert.AreEqual(1, Utility.MatchCount(0b0000_0000_0000_0000, 0b1111_1111_1111_1110));
            Assert.AreEqual(2, Utility.MatchCount(0b0000_0000_0000_0000, 0b1111_1111_1011_1011));
            Assert.AreEqual(3, Utility.MatchCount(0b0000_0000_0000_0000, 0b1111_1111_1101_0110));
            Assert.AreEqual(4, Utility.MatchCount(0b0000_0000_0000_0000, 0b1111_1111_0101_0110));

            Assert.AreEqual(0, Utility.MatchCount(0b1111_1111_1111_1111, 0b0000_0000_0000_0000));
            Assert.AreEqual(1, Utility.MatchCount(0b1111_1111_1111_1111, 0b0000_0000_0000_0100));
            Assert.AreEqual(2, Utility.MatchCount(0b1111_1111_1111_1111, 0b0000_0000_1000_0100));
            Assert.AreEqual(3, Utility.MatchCount(0b1111_1111_1111_1111, 0b0000_0000_1000_1100));
            Assert.AreEqual(4, Utility.MatchCount(0b1111_1111_1111_1111, 0b0000_0000_1001_1100));
        }
    }
}