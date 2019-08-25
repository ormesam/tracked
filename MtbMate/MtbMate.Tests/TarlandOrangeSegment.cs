using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtbMate.Utilities;

namespace MtbMate.Tests
{
    [TestClass]
    public class TarlandOrangeSegment
    {
        [TestMethod]
        public void Tarland_Orange_Blue1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandBlue1));
        }

        [TestMethod]
        public void Tarland_Orange_Blue2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandBlue2));
        }

        [TestMethod]
        public void Tarland_Orange_Blue3() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandBlue3));
        }

        [TestMethod]
        public void Tarland_Orange_Blue4() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandBlue4));
        }

        [TestMethod]
        public void Tarland_Orange_Red1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandRed1));
        }

        [TestMethod]
        public void Tarland_Orange_Red2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandRed2));
        }

        [TestMethod]
        public void Tarland_Orange_Orange1() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandOrange1));
        }

        [TestMethod]
        public void Tarland_Orange_Orange2() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandOrangeSegment, TestSegments.TarlandOrange2));
        }
    }
}
