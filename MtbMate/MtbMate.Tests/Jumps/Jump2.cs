using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump2 : JumpTestBase {
        public override string FileName => "Jumps\\Jump2.csv";

        [TestMethod]
        public void Analyse() {
            Assert.IsTrue(JumpDetectionUtility.Jumps.Count == 2);

            Assert.AreEqual(0.683, JumpDetectionUtility.Jumps[0].Airtime, 0.0001);
            Assert.AreEqual(1.2, JumpDetectionUtility.Jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.781, JumpDetectionUtility.Jumps[1].Airtime, 0.0001);
            Assert.AreEqual(5.9, JumpDetectionUtility.Jumps[1].LandingGForce, 0.1);
        }
    }
}
