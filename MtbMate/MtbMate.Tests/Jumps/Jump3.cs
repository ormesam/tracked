using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump3 : JumpTestBase {
        public override string FileName => "Jumps\\Jump3.csv";

        [TestMethod]
        public void Analyse() {
            var jumps = JumpDetectionUtility.Run();

            Assert.IsTrue(jumps.Count == 4);

            Assert.AreEqual(0.537, jumps[0].Airtime, 0.0001);
            Assert.AreEqual(2, jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.877, jumps[1].Airtime, 0.0001);
            Assert.AreEqual(3.3, jumps[1].LandingGForce, 0.1);

            Assert.AreEqual(0.534, jumps[2].Airtime, 0.0001);
            Assert.AreEqual(5.5, jumps[2].LandingGForce, 0.1);

            Assert.AreEqual(0.78, jumps[3].Airtime, 0.0001);
            Assert.AreEqual(1.1, jumps[3].LandingGForce, 0.1);
        }
    }
}
