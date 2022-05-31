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
    public class Argb2222Tests
    {
        [TestMethod()]
        public void ConstructorTest()
        {
            Argb2222 argb = new Argb2222(Rgb.Red);
            Assert.AreEqual(Argb2222.Red, argb);
            argb = new Argb2222(Rgb.Green);
            Assert.AreEqual(Argb2222.Green, argb);
            argb = new Argb2222(Rgb.Blue);
            Assert.AreEqual(Argb2222.Blue, argb);
            argb = new Argb2222(Rgb.White);
            Assert.AreEqual(Argb2222.White, argb);
            argb = new Argb2222(Rgb.Black);
            Assert.AreEqual(Argb2222.Black, argb);

            argb = new Argb2222(1, 0, 0, 0);
            Assert.AreEqual(0b01_00_00_00, argb.Bits);
            argb = new Argb2222(0, 1, 0, 0);
            Assert.AreEqual(0b00_01_00_00, argb.Bits);
            argb = new Argb2222(0, 0, 1, 0);
            Assert.AreEqual(0b00_00_01_00, argb.Bits);
            argb = new Argb2222(0, 0, 0, 1);
            Assert.AreEqual(0b00_00_00_01, argb.Bits);

            argb = new Argb2222(1, 0, 0);
            Assert.AreEqual(0b11_01_00_00, argb.Bits);
            argb = new Argb2222(0, 1, 0);
            Assert.AreEqual(0b11_00_01_00, argb.Bits);
            argb = new Argb2222(0, 0, 1);
            Assert.AreEqual(0b11_00_00_01, argb.Bits);
        }
        [TestMethod()]
        public void PropertiesTest()
        {
            // 設定した値が正しいかチェック
            // 他のビットを汚染していないかチェック
            Argb2222 argb = Argb2222.White;
            for (int i = 0; i <= Argb2222.AMax; i++)
            {
                argb.A = i;
                Assert.AreEqual(i, argb.A);
                Assert.AreEqual(Argb2222.RMax, argb.R);
                Assert.AreEqual(Argb2222.GMax, argb.G);
                Assert.AreEqual(Argb2222.BMax, argb.B);
            }
            for (int i = 0; i <= Argb2222.RMax; i++)
            {
                argb.R = i;
                Assert.AreEqual(i, argb.R);
                Assert.AreEqual(Argb2222.AMax, argb.A);
                Assert.AreEqual(Argb2222.GMax, argb.G);
                Assert.AreEqual(Argb2222.BMax, argb.B);
            }
            for (int i = 0; i <= Argb2222.GMax; i++)
            {
                argb.G = i;
                Assert.AreEqual(i, argb.G);
                Assert.AreEqual(Argb2222.AMax, argb.A);
                Assert.AreEqual(Argb2222.RMax, argb.R);
                Assert.AreEqual(Argb2222.BMax, argb.B);
            }
            for (int i = 0; i <= Argb2222.BMax; i++)
            {
                argb.B = i;
                Assert.AreEqual(i, argb.B);
                Assert.AreEqual(Argb2222.AMax, argb.A);
                Assert.AreEqual(Argb2222.RMax, argb.R);
                Assert.AreEqual(Argb2222.GMax, argb.G);
            }

            argb = Argb2222.Zero;
            for (int i = 0; i <= Argb2222.RMax; i++)
            {
                argb.A = i;
                Assert.AreEqual(i, argb.A);
                Assert.AreEqual(0, argb.R);
                Assert.AreEqual(0, argb.G);
                Assert.AreEqual(0, argb.B);
            }
            argb = Argb2222.Zero;
            for (int i = 0; i <= Argb2222.RMax; i++)
            {
                argb.R = i;
                Assert.AreEqual(0, argb.A);
                Assert.AreEqual(i, argb.R);
                Assert.AreEqual(0, argb.G);
                Assert.AreEqual(0, argb.B);
            }
            argb = Argb2222.Zero;
            for (int i = 0; i <= Argb2222.GMax; i++)
            {
                argb.G = i;
                Assert.AreEqual(0, argb.A);
                Assert.AreEqual(0, argb.R);
                Assert.AreEqual(i, argb.G);
                Assert.AreEqual(0, argb.B);
            }
            argb = Argb2222.Zero;
            for (int i = 0; i <= Argb2222.BMax; i++)
            {
                argb.B = i;
                Assert.AreEqual(0, argb.A);
                Assert.AreEqual(0, argb.R);
                Assert.AreEqual(0, argb.G);
                Assert.AreEqual(i, argb.B);
            }
        }

        [TestMethod()]
        public void OperatorsTest()
        {
            Argb2222 argb;
            for (int i = 0; i <= byte.MaxValue; i++)
            {
                argb = (byte)i;
                byte bits = (byte)argb;
                Assert.AreEqual(i, bits);
            }

            {
                argb = new Argb2222(1, 1, 1, 1);
                Color color = (Color)argb;
                Assert.AreEqual(0b01010101, color.A);
                Assert.AreEqual(0b01010101, color.R);
                Assert.AreEqual(0b01010101, color.G);
                Assert.AreEqual(0b01010101, color.B);
            }

            {
                argb = new Argb2222(2, 2, 2, 2);
                Color color = (Color)argb;
                Assert.AreEqual(0b10101010, color.A);
                Assert.AreEqual(0b10101010, color.R);
                Assert.AreEqual(0b10101010, color.G);
                Assert.AreEqual(0b10101010, color.B);
            }

            {
                argb = new Argb2222(3, 3, 3, 3);
                Color color = (Color)argb;
                Assert.AreEqual(0b11111111, color.A);
                Assert.AreEqual(0b11111111, color.R);
                Assert.AreEqual(0b11111111, color.G);
                Assert.AreEqual(0b11111111, color.B);
            }

            {
                argb = (Argb2222)Color.Red;
                Color color = (Color)argb;
                Assert.AreEqual(Color.Red.A, color.A);
                Assert.AreEqual(Color.Red.R, color.R);
                Assert.AreEqual(Color.Red.G, color.G);
                Assert.AreEqual(Color.Red.B, color.B);
            }

            {
                argb = (Argb2222)Color.White;
                Color color = (Color)argb;
                Assert.AreEqual(Color.White.A, color.A);
                Assert.AreEqual(Color.White.R, color.R);
                Assert.AreEqual(Color.White.G, color.G);
                Assert.AreEqual(Color.White.B, color.B);
            }

            {
                argb = (Argb2222)Color.Blue;
                Color color = (Color)argb;
                Assert.AreEqual(Color.Blue.A, color.A);
                Assert.AreEqual(Color.Blue.R, color.R);
                Assert.AreEqual(Color.Blue.G, color.G);
                Assert.AreEqual(Color.Blue.B, color.B);
            }
        }
    }
}