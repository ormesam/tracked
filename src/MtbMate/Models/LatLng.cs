using System.Diagnostics;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [DebuggerDisplay("Lat: {Latitude} Lng: {Longitude}")]
    public class LatLng {
        [JsonProperty]
        public double Latitude { get; set; }
        [JsonProperty]
        public double Longitude { get; set; }

        public LatLng(double lat, double lng) {
            Latitude = lat;
            Longitude = lng;
        }
    }
}
