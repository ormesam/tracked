namespace MtbMate.Models
{
    public class LatLongModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Speed { get; set; }

        public LatLongModel() { }

        public LatLongModel(double lat, double lng, double? speed = null) {
            Latitude = lat;
            Longitude = lng;
            Speed = speed;
        }
    }
}
