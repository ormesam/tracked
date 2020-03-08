using System.Diagnostics;
using Newtonsoft.Json;
using Shared.Interfaces;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    [DebuggerDisplay("Lat: {Latitude} Lng: {Longitude}")]
    public class LatLng : ILatLng {
        [JsonProperty]
        public decimal Latitude { get; set; }
        [JsonProperty]
        public decimal Longitude { get; set; }

        public LatLng(double lat, double lng) {
            Latitude = (decimal)lat;
            Longitude = (decimal)lng;
        }
    }
}
