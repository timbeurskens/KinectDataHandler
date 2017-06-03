using System;
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
        public void TestPlane3D1()
        {
            var p = new Plane3D(1, 0, 0, 0);
            Assert.AreEqual(new Vector3D(1, 0, 0), p.Normal());
        }

        [TestMethod]
        public void TestPlane3D2()
        {
            var v = new Vector3D(1, 0, 0);
            var po = new Vector3D(0, 0, 0);
            var p = new Plane3D(v, po);
            Assert.AreEqual(new Vector3D(1, 0, 0), p.Normal());
        }

        [TestMethod]
        public void TestPlane3D3()
        {
            var v1 = new Vector3D(1, 0, 0);
            var v2 = new Vector3D(0, 1, 0);
            var p0 = new Vector3D(0) as IPoint3D;
            var p = new Plane3D(v1, v2, p0);
            Assert.AreEqual(new Vector3D(0, 0, 1), p.Normal());
        }

        [TestMethod]
        public void TestPlane3D4()
        {
            var p1 = new Vector3D(1, 0, 0) as IPoint3D;
            var p2 = new Vector3D(0, 1, 0) as IPoint3D;
            var p0 = new Vector3D(0) as IPoint3D;
            var p = new Plane3D(p0, p1, p2);
            Assert.AreEqual(new Vector3D(0, 0, 1), p.Normal());
        }

        [TestMethod]
        public void RandomizedCrossProductTest()
        {
            var r = new Random();
            for (var i = 0; i < 10; i++)
            {
                var a = new Vector3D(r.Next(100), r.Next(100), r.Next(100));
                var b = new Vector3D(r.Next(100), r.Next(100), r.Next(100));
                var crossA = (Vector3D) a.Cross(b);
                var crossB = (Vector3D) b.Cross(a);

                Console.WriteLine(a);
                Console.WriteLine(b);

                Console.WriteLine(crossA);
                Console.WriteLine(crossB);

                Assert.IsTrue(EqualVectors(crossB.Invert() as Vector3D, crossA));
                Assert.IsTrue(crossB.Dot(a) < 0.01);
                Assert.IsTrue(crossA.Dot(a) < 0.01);
                Assert.IsTrue(crossB.Dot(b) < 0.01);
                Assert.IsTrue(crossA.Dot(b) < 0.01);
            }
        }

        public bool EqualVectors(Vector3D a, Vector3D b)
        {
            return Math.Abs(a.X - b.X) < 0.1 && Math.Abs(a.Y - b.Y) < 0.1 && Math.Abs(a.Z - b.Z) < 0.1;
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
            var a = Vector3D.XUnit3D;
            var b = Vector3D.YUnit3D;
            var c = Vector3D.ZUnit3D;
            Assert.AreEqual(Vector3D.Unit3D, a + b + c);
        }
    }
}