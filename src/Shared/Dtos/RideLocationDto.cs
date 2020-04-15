using System;
using Shared.Interfaces;

namespace Shared.Dtos {
    public class RideLocationDto : ILatLng {
        public int RideLocationId { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double AccuracyInMetres { get; set; }
        public double SpeedMetresPerSecond { get; set; }
        public double Altitude { get; set; }

        public double Mph => SpeedMetresPerSecond * 2.23694;

        public override string ToString() {
            return $"{Timestamp}: Lat: {Latitude}, Lon: {Longitude}, Mph: {Mph}, Accuracy: {AccuracyInMetres}";
        }
    }
}
