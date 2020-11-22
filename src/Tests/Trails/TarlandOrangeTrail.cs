using Api.Analysers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Trails {
    [TestClass]
    public class TarlandOrangeTrail {
        private TrailAnalyser trailAnalyser;

        [TestInitialize]
        public void Initialize() {
            trailAnalyser = new TrailAnalyser();
        }

        [TestMethod]
        public void Tarland_Orange_Blue1() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandBlue1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Blue2() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandBlue2).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Blue3() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandBlue3).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Blue4() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandBlue4).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Red1() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandRed1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Red2() {
            Assert.IsFalse(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandRed2).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Orange1() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandOrange1).MatchesTrail);
        }

        [TestMethod]
        public void Tarland_Orange_Orange2() {
            Assert.IsTrue(trailAnalyser.LocationsMatch(TestTrails.TarlandOrangeTrail, TestTrails.TarlandOrange2).MatchesTrail);
        }
    }
}
