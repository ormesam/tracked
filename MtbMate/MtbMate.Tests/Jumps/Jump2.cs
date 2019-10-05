using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MtbMate.Tests.Jumps {
    [TestClass]
    public class Jump2 : JumpTestBase {
        public override string FileName => "Jumps\\Jump2.csv";

        [TestMethod]
        public void Analyse() {
            JumpDetectionUtility.Run();

            Assert.IsTrue(Ride.Jumps.Count == 2);

            Assert.AreEqual(0.683, Ride.Jumps[0].Airtime, 0.0001);
            Assert.AreEqual(1.2, Ride.Jumps[0].LandingGForce, 0.1);

            Assert.AreEqual(0.781, Ride.Jumps[1].Airtime, 0.0001);
            Assert.AreEqual(5.9, Ride.Jumps[1].LandingGForce, 0.1);
        }
    }
}
