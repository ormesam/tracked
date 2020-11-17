using Api.Analysers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Trails {
    [TestClass]
    public class TarlandBlueTrail {
        private TrailAnalyser trailAnalyser;

        [TestInitialize]
        public void Initialize() {
            trailAnalyser = new TrailAnalyser();
        }

        [TestMethod]
        public void Tarland_Blue_Blue1() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandBlue1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Blue2() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandBlue2).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Blue3() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandBlue3).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Blue4() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandBlue4).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Red1() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandRed1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Red2() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandRed2).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Orange1() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandOrange1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Blue_Orange2() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandBlueTrail, TestTrails.TarlandOrange2).MatchesTrail);
        }
    }
}
