using Shared.Interfaces;

namespace Tests.Trails {
    public class TrailLocation : ILatLng {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public TrailLocation(int number, double lat, double lng) {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
