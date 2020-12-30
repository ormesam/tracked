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
        public void Kirkhill() {
        }
    }
}
