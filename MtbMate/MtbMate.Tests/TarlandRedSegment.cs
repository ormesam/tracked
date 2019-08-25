using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtbMate.Utilities;

namespace MtbMate.Tests
{
    [TestClass]
    public class TarlandRedSegment
    {
        [TestMethod]
        public void Tarland_Red_Blue1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue1));
        }

        [TestMethod]
        public void Tarland_Red_Blue2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue2));
        }

        [TestMethod]
        public void Tarland_Red_Blue3() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue3));
        }

        [TestMethod]
        public void Tarland_Red_Blue4() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue4));
        }

        [TestMethod]
        public void Tarland_Red_Red1() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandRed1));
        }

        [TestMethod]
        public void Tarland_Red_Red2() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandRed2));
        }

        [TestMethod]
        public void Tarland_Red_Orange1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandOrange1));
        }

        [TestMethod]
        public void Tarland_Red_Orange2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandOrange2));
        }
    }
}
