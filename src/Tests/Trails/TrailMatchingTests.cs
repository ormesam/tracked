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
        public void Kirkhill_FunPark() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillFunPark });

            Assert.IsTrue(results.All(i => i.TrailId == TrailData.KirkhillFunPark.TrailId));
            // There are 4 loops of the track in the GPS data,
            // however the gps data in one is a bit off due to GPS drift, meaning we can only match 3
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void Kirkhill_FunParkUp() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillFunParkUp });

            Assert.IsTrue(results.All(i => i.TrailId == TrailData.KirkhillFunParkUp.TrailId));
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void Kirkhill_FunParkLoop() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillFunParkLoop });

            Assert.IsTrue(results.All(i => i.TrailId == TrailData.KirkhillFunParkLoop.TrailId));
            Assert.AreEqual(3, results.Count);
        }

        [TestMethod]
        public void Kirkhill_Dummy() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] { TrailData.KirkhillDummy });

            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void Kirkhill_All() {
            var results = trailAnalyser.Analyse(RideData.KirkhillRide, new[] {
                TrailData.KirkhillFunPark,
                TrailData.KirkhillFunParkLoop,
                TrailData.KirkhillFunParkUp,
                TrailData.KirkhillDummy,
            });

            Assert.AreEqual(4, results.Count(i => i.TrailId == TrailData.KirkhillFunPark.TrailId));
            Assert.AreEqual(3, results.Count(i => i.TrailId == TrailData.KirkhillFunParkLoop.TrailId));
            Assert.AreEqual(3, results.Count(i => i.TrailId == TrailData.KirkhillFunParkUp.TrailId));

            Assert.AreEqual(10, results.Count);
        }
    }
}
