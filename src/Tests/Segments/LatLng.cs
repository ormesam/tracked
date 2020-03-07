using Shared.Interfaces;

namespace Tests.Segments {
    public class LatLng : ILatLng {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public LatLng(double latitude, double longitude) {
            Latitude = (decimal)latitude;
            Longitude = (decimal)longitude;
        }
    }
}
