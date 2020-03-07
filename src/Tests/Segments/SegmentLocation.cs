using Shared.Interfaces;

namespace Tests.Segments {
    public class SegmentLocation : ILatLng {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public SegmentLocation(int number, double lat, double lng) {
            Latitude = (decimal)lat;
            Longitude = (decimal)lng;
        }
    }
}
