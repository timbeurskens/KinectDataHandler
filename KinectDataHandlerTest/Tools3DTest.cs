using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KinectDataHandler.Linear3DTools;

namespace KinectDataHandlerTest
{
    [TestClass]
    public class Tools3DTest
    {
        [TestMethod]
        public void TestDotProduct()
        {
            var a = new Vector3D(1, 0, 0);
            var b = new Vector3D(0, 0, 1);

            Assert.AreEqual(0, a * b);
            Assert.AreEqual(1, a * a);
            Assert.AreEqual(1, b * b);
        }

        [TestMethod]
        public void RandomizedCrossProductTest()
        {
            var r = new Random();
            for (var i = 0; i < 100; i++)
            {
                Vector3D a = new Vector3D(r.Next(100), r.Next(100), r.Next(100));
                Vector3D b = new Vector3D(r.Next(100), r.Next(100), r.Next(100));
                Vector3D cross_a = (Vector3D)a.Cross(b);
                Vector3D cross_b = (Vector3D)b.Cross(a);

                Console.WriteLine(Vector3D.XUnit3D);
                Console.WriteLine(Vector3D.YUnit3D);
                Console.WriteLine(Vector3D.ZUnit3D);

                Console.WriteLine(a);
                Console.WriteLine(b);

                Console.WriteLine(cross_a);
                Console.WriteLine(cross_b);

                //Assert.IsTrue(EqualVectors(cross_b.Invert() as Vector3D, cross_a));
                Assert.IsTrue(cross_b.Dot(a) < 0.1);
                Assert.IsTrue(cross_a.Dot(a) < 0.1);
                Assert.IsTrue(cross_b.Dot(b) < 0.1);
                Assert.IsTrue(cross_a.Dot(b) < 0.1);
            }
        }

        public bool EqualVectors(Vector3D a, Vector3D b)
        {
            return Math.Abs(a.X - b.X) < 0.1 && Math.Abs(a.Y - b.Y) < 0.1 && Math.Abs(a.Z - b.Z) < 0.1;
        }

        [TestMethod]
        public void TestDictionary()
        {
            IDictionary<int, int> dict = new Dictionary<int, int>();
            for (var i = 0; i < 10; i++)
            {
                dict.Add(i, i);
                
            }
            Assert.AreEqual(10, dict.Count);
            for (var i = 0; i < 10; i++)
            {
                dict.Add(i, i * 2);
            }
            Assert.AreEqual(10, dict.Count);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var a = new Vector3D(5);
            
            Assert.AreEqual(5, a.X);
            Assert.AreEqual(5, a.Y);
            Assert.AreEqual(5, a.Z);

        }

        [TestMethod]
        public void TestCrossProduct()
        {
//            var a = new Vector3D(1, 0, 0);
//            var b = new Vector3D(0, 0, 1);
//
//            Assert.AreEqual(0, a.Cross(b));
//            Assert.AreEqual(0, b.Cross(a));
//            Assert.AreEqual(1, a.Cross(a));
//            Assert.AreEqual(1, b.Cross(b));
        }

        [TestMethod]
        public void TestCrossProductSpecial1()
        {
            Assert.AreEqual(Vector3D.ZUnit3D, Vector3D.XUnit3D.Cross(Vector3D.YUnit3D));
        }

        [TestMethod]
        public void TestCrossProductSpecial2()
        {
            Assert.AreEqual(Vector3D.XUnit3D, Vector3D.YUnit3D.Cross(Vector3D.ZUnit3D));
        }

        [TestMethod]
        public void TestCrossProductSpecial3()
        {
            Assert.AreEqual(Vector3D.YUnit3D, Vector3D.ZUnit3D.Cross(Vector3D.XUnit3D));
        }

        [TestMethod]
        public void TestAdd()
        {
            Vector3D a = Vector3D.XUnit3D;
            Vector3D b = Vector3D.YUnit3D;
            Vector3D c = Vector3D.ZUnit3D;
            Assert.AreEqual(Vector3D.Unit3D, a + b + c);
        }
    }
}
