using Api.Analysers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Trails {
    [TestClass]
    public class TarlandRedTrail {
        private TrailAnalyser trailAnalyser;

        [TestInitialize]
        public void Initialize() {
            trailAnalyser = new TrailAnalyser();
        }

        [TestMethod]
        public void Tarland_Red_Blue1() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandBlue1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Blue2() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandBlue2).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Blue3() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandBlue3).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Blue4() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandBlue4).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Red1() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandRed1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Red2() {
            // This actually does not match the red trail, looks like the signal bounced off the trees
            // or changed satalite because the second half of the ride is way off.
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandRed2).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Orange1() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandOrange1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Red_Orange2() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandRedTrail, TestTrails.TarlandOrange2).MatchesTrail);
        }
    }
}
