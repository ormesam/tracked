using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SegmentLocation {
        [JsonProperty]
        public int Order { get; set; }
        [JsonProperty]
        public LatLng Point { get; set; }

        public SegmentLocation(int order, double lat, double lng) {
            Order = order;
            Point = new LatLng(lat, lng);
        }
    }
}
