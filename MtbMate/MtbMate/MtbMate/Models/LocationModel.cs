using System;

namespace MtbMate.Models
{
    public class LocationModel
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MetresPerSecond { get; set; }

        public int Mph => (int)Math.Round(MetresPerSecond * 2.2369363);
    }
}
