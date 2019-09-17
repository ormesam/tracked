using System.Diagnostics;

namespace MtbMate.Models {
    [DebuggerDisplay("Lat: {Latitude} Lng: {Longitude}")]
    public class LatLng {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LatLng(double lat, double lng) {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
