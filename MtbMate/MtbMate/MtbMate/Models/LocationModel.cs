using System;

namespace MtbMate.Models
{
    public class LocationModel
    {
        public DateTime Timestamp { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public override string ToString()
        {
            return $"{Timestamp}: Lat: {Latitude}, Lon: {Longitude}";
        }
    }
}
