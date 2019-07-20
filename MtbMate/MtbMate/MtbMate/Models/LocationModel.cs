using System;
using GeoCoordinatePortable;

namespace MtbMate.Models
{
    public class LocationModel
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double MetresPerSecond { get; set; }

        public int Mph => (int)Math.Round(MetresPerSecond * 2.2369363);

        public double DistanceBetween(LocationModel other)
        {
            GeoCoordinate pin1 = new GeoCoordinate(Latitude, Longitude);
            GeoCoordinate pin2 = new GeoCoordinate(other.Latitude, other.Longitude);

            return pin1.GetDistanceTo(pin2) / 1000;
        }

        public override string ToString()
        {
            return $"{Timestamp}: Lat: {Latitude}, Lon: {Longitude}, Speed: {Mph}mph";
        }
    }
}
