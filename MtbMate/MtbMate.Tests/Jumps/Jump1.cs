using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump1 : JumpTestBase {
        public override string FileName => "Jumps\\Jump1.csv";

        [TestMethod]
        public void Analyse() {
            var jumps = JumpDetectionUtility.Run();

            Assert.IsTrue(jumps.Count == 2);

            Assert.AreEqual(0.539, jumps[0].Airtime, 0.0001);
            Assert.AreEqual(3.7, jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.534, jumps[1].Airtime, 0.0001);
            Assert.AreEqual(2.2, jumps[1].LandingGForce, 0.1);
        }
    }
}
