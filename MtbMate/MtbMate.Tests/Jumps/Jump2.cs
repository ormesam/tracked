using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump2 : JumpTestBase {
        public override string FileName => "Jumps\\Jump2.csv";

        [TestMethod]
        public void Analyse() {
            var jumps = JumpDetectionUtility.Run();

            Assert.IsTrue(jumps.Count == 2);

            Assert.AreEqual(0.683, jumps[0].Airtime, 0.0001);
            Assert.AreEqual(1.2, jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.781, jumps[1].Airtime, 0.0001);
            Assert.AreEqual(5.9, jumps[1].LandingGForce, 0.1);
        }
    }
}
