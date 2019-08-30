using System.Diagnostics;

namespace MtbMate.Models
{
    [DebuggerDisplay("Lat: {Latitude} Lng: {Longitude}")]
    public class LatLngModel
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public LatLngModel(double lat, double lng) {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
