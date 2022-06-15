using MosaicArt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;

namespace MosaicArt.Tests
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
        public void SquaredDistanceTest()
        {
            // Bitmap
            {
#pragma warning disable CA1416 // プラットフォームの互換性を検証
                Bitmap bitmap0 = new(1, 1);
                Bitmap bitmap1 = new(1, 1);
                bitmap0.SetPixel(0, 0, Color.Black);
                bitmap1.SetPixel(0, 0, Color.Black);
                var squaredDistance = Utility.SquaredDistance(bitmap0, bitmap1);
                Assert.AreEqual(0, squaredDistance);

                bitmap0.SetPixel(0, 0, Color.Black);
                bitmap1.SetPixel(0, 0, Color.Red);
                squaredDistance = Utility.SquaredDistance(bitmap0, bitmap1);
                Assert.AreEqual(255 * 255 * 1, squaredDistance);

                bitmap0.SetPixel(0, 0, Color.Black);
                bitmap1.SetPixel(0, 0, Color.White);
                squaredDistance = Utility.SquaredDistance(bitmap0, bitmap1);
                Assert.AreEqual(255 * 255 * 3, squaredDistance);
#pragma warning restore CA1416 // プラットフォームの互換性を検証
            }
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
        [TestMethod()]
        public void ShuffleTest()
        {
            List<int> list = new List<int>();
            list.Add(1);
            list.Add(2);
            list.Add(3);
            Utility.Shuffle(list);
        }
    }
}