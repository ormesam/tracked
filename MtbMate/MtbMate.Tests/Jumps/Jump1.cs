using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump1 : JumpTestBase {
        public override string FileName => "Jumps\\Jump1.csv";

        [TestMethod]
        public void Analyse() {
            JumpDetectionUtility.Run();

            Assert.IsTrue(Ride.Jumps.Count == 2);

            Assert.AreEqual(0.539, Ride.Jumps[0].Airtime, 0.0001);
            Assert.AreEqual(3.7, Ride.Jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.534, Ride.Jumps[1].Airtime, 0.0001);
            Assert.AreEqual(2.2, Ride.Jumps[1].LandingGForce, 0.1);
        }
    }
}
