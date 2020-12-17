using System;
using Shared.Interfaces;

namespace Shared.Dtos {
    public class JumpDto : ILatLng {
        public int JumpId { get; set; }
        public int Number { get; set; }
        public DateTime Timestamp { get; set; }
        public double Airtime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
