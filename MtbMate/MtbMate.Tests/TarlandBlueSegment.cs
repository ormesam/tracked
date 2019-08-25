using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MtbMate.Utilities;

namespace MtbMate.Tests
{
    [TestClass]
    public class TarlandBlueSegment
    {
        [TestMethod]
        public void Tarland_Blue_Blue1() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue1));
        }

        [TestMethod]
        public void Tarland_Blue_Blue2() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue2));
        }

        [TestMethod]
        public void Tarland_Blue_Blue3() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue3));
        }

        [TestMethod]
        public void Tarland_Blue_Blue4() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue4));
        }

        [TestMethod]
        public void Tarland_Blue_Red1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandRed1));
        }

        [TestMethod]
        public void Tarland_Blue_Red2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandRed2));
        }

        [TestMethod]
        public void Tarland_Blue_Orange1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandOrange1));
        }

        [TestMethod]
        public void Tarland_Blue_Orange2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandOrange2));
        }
    }
}
