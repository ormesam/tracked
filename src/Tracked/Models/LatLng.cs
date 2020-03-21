using Shared.Interfaces;

namespace Tracked.Models {
    public class LatLng : ILatLng {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LatLng(double lat, double lng) {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
