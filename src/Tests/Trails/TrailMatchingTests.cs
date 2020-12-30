using System.Linq;
using Api.Analysers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Tests.Trails {
    [TestClass]
    public class TrailMatchingTests {
        private TrailAnalyser trailAnalyser;

        [TestInitialize]
        public void Initialize() {
            trailAnalyser = new TrailAnalyser();
        }

        [TestMethod]
        public void Kirkhill_Funpark() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillFunPark });

            Assert.IsTrue(results.All(i => i.TrailId == TrailData.KirkhillFunPark.TrailId));
            Assert.AreEqual(4, results.Count());
        }

        [TestMethod]
        public void Kirkhill_FunparkUp() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillFunParkUp });

            Assert.IsTrue(results.All(i => i.TrailId == TrailData.KirkhillFunParkUp.TrailId));
            Assert.AreEqual(3, results.Count());
        }

        [TestMethod]
        public void Kirkhill_Dummy() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillDummy });

            Assert.AreEqual(0, results.Count());
        }
    }
}
