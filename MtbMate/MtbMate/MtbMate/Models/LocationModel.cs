using System;

namespace MtbMate.Models
{
    public class LocationModel
    {
        public long Time { get; set; }
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public float? AccuracyInMetres { get; set; }
        public float? SpeedMetresPerSecond { get; set; }
        public float? SpeedAccuracyMetresPerSecond { get; set; }
        public double? Altitude { get; set; }

        public override string ToString()
        {
            return $"{Timestamp}: Lat: {Latitude}, Lon: {Longitude}";
        }
    }
}
