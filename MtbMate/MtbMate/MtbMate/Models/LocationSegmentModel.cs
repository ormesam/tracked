using GeoCoordinatePortable;

namespace MtbMate.Models
{
    public class LocationSegmentModel
    {
        public LocationModel Start { get; set; }
        public LocationModel End { get; set; }
        public double Distance { get; set; }
        public double Mph { get; set; }

        public double CalculateValues()
        {
            GeoCoordinate pin1 = new GeoCoordinate(Start.Latitude, Start.Longitude);
            GeoCoordinate pin2 = new GeoCoordinate(End.Latitude, End.Longitude);

            var km = pin2.GetDistanceTo(pin1) / 1000;
            var miles = km * 0.621371192;

            var speed = miles / (End.Timestamp - Start.Timestamp).TotalHours;

            Mph = speed;

            return miles;
        }
    }
}
