using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump3 : JumpTestBase {
        public override string FileName => "Jumps\\Jump3.csv";

        [TestMethod]
        public void Analyse() {
            Assert.IsTrue(JumpDetectionUtility.Jumps.Count == 4);

            Assert.AreEqual(0.686, JumpDetectionUtility.Jumps[0].Airtime, 0.0001);
            Assert.AreEqual(1.82, JumpDetectionUtility.Jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.877, JumpDetectionUtility.Jumps[1].Airtime, 0.0001);
            Assert.AreEqual(4.45, JumpDetectionUtility.Jumps[1].LandingGForce, 0.1);

            Assert.AreEqual(0.534, JumpDetectionUtility.Jumps[2].Airtime, 0.0001);
            Assert.AreEqual(7.27, JumpDetectionUtility.Jumps[2].LandingGForce, 0.1);

            Assert.AreEqual(0.634, JumpDetectionUtility.Jumps[3].Airtime, 0.0001);
            Assert.AreEqual(1.7, JumpDetectionUtility.Jumps[3].LandingGForce, 0.1);
        }
    }
}
