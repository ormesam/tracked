namespace MtbMate.Tests
{
    public class LatLng
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double? Speed { get; set; }

        public LatLng(double lat, double lng, double? speed = null) {
            Lat = lat;
            Lng = lng;
            Speed = speed;
        }
    }
}
