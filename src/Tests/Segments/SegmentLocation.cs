using Shared.Interfaces;

namespace Tests.Segments {
    public class SegmentLocation : ILatLng {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public SegmentLocation(int number, double lat, double lng) {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
