using System;

namespace MtbMate.Models
{
    public class LocationModel
    {
        public long Time { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? AccuracyInMetres { get; set; }
        public double SpeedMetresPerSecond { get; set; }
        public double? SpeedAccuracyMetresPerSecond { get; set; }
        public double? Altitude { get; set; }

        public double Mph => SpeedMetresPerSecond * 2.23694;

        public override string ToString()
        {
            return $"{Timestamp}: Lat: {Latitude}, Lon: {Longitude}, Mph: {Mph}";
        }
    }
}
