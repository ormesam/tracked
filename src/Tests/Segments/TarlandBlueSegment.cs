using Api.Analysers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Segments {
    [TestClass]
    public class TarlandBlueSegment {
        private SegmentAnalyser segmentAnalyser;

        [TestInitialize]
        public void Initialize() {
            segmentAnalyser = new SegmentAnalyser();
        }

        [TestMethod]
        public void Tarland_Blue_Blue1() {
            Assert.IsTrue(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue1).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Blue2() {
            Assert.IsTrue(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue2).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Blue3() {
            Assert.IsTrue(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue3).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Blue4() {
            Assert.IsTrue(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandBlue4).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Red1() {
            Assert.IsFalse(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandRed1).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Red2() {
            Assert.IsFalse(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandRed2).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Orange1() {
            Assert.IsFalse(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandOrange1).MatchesSegment);
        }

        [TestMethod]
        public void Tarland_Blue_Orange2() {
            Assert.IsFalse(segmentAnalyser.LocationsMatch(TestSegments.TarlandBlueSegment, TestSegments.TarlandOrange2).MatchesSegment);
        }
    }
}
