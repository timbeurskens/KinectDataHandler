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
            Assert.AreEqual(Vector3D.ZUnit3D, Vector3D.XUnit3D.Cross(Vector3D.YUnit3D).Invert());
        }

        [TestMethod]
        public void TestCrossProductSpecial2()
        {
            Assert.AreEqual(Vector3D.XUnit3D, Vector3D.YUnit3D.Cross(Vector3D.ZUnit3D).Invert());
        }

        [TestMethod]
        public void TestCrossProductSpecial3()
        {
            Assert.AreEqual(Vector3D.YUnit3D, Vector3D.ZUnit3D.Cross(Vector3D.XUnit3D).Invert());
        }
    }
}
