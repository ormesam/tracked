using System;

namespace Shared.Dtos {
    public class JumpDto {
        public int Number { get; set; }
        public DateTime Time { get; set; }
        public decimal Airtime { get; set; }
        public decimal LandingGForce { get; set; }
    }
}
