using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tracked.Utilities;

namespace Tracked.Tests.Segments {
    [TestClass]
    public class TarlandRedSegment {
        [TestMethod]
        public void Tarland_Red_Blue1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue1).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Blue2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue2).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Blue3() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue3).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Blue4() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandBlue4).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Red1() {
            Assert.IsTrue(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandRed1).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Red2() {
            // This actually does not match the red segment, looks like the signal bounced off the trees
            // or changed satalite because the second half of the ride is way off.
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandRed2).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Orange1() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandOrange1).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Red_Orange2() {
            Assert.IsFalse(PolyUtils.LocationsMatch(TestSegments.TarlandRedSegment, TestSegments.TarlandOrange2).MatchesSegment);
        }
    }
}
