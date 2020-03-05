using System;
using Shared.Interfaces;

namespace Shared.Dtos {
    public class RideLocationDto : ILatLng {
        public int RideLocationId { get; set; }
        public int RideId { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal AccuracyInMetres { get; set; }
        public decimal SpeedMetresPerSecond { get; set; }
        public decimal Altitude { get; set; }
    }
}
