using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump1 : JumpTestBase {
        public override string FileName => "Jumps\\Jump1.csv";

        [TestMethod]
        public void Analyse() {
            Assert.IsTrue(JumpDetectionUtility.Jumps.Count == 2);

            Assert.AreEqual(0.536, JumpDetectionUtility.Jumps[0].Airtime, 0.0001);
            Assert.AreEqual(6.63, JumpDetectionUtility.Jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.587, JumpDetectionUtility.Jumps[1].Airtime, 0.0001);
            Assert.AreEqual(1.36, JumpDetectionUtility.Jumps[1].LandingGForce, 0.1);
        }
    }
}
