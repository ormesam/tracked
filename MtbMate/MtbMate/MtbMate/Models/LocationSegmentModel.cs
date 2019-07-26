using GeoCoordinatePortable;

namespace MtbMate.Models
{
    public class LocationSegmentModel
    {
        public LocationModel Start { get; set; }
        public LocationModel End { get; set; }
        public double Distance => CalculateDistance();
        public double Mph => End.Mph;

        public double CalculateDistance()
        {
            GeoCoordinate pin1 = new GeoCoordinate(Start.Latitude, Start.Longitude);
            GeoCoordinate pin2 = new GeoCoordinate(End.Latitude, End.Longitude);

            var km = pin2.GetDistanceTo(pin1) / 1000;
            var miles = km * 0.621371192;

            return miles;
        }
    }
}
