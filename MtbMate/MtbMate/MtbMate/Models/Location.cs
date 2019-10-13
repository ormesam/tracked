using System;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Location {
        [JsonProperty]
        public long Time { get; set; }
        [JsonProperty]
        public DateTime Timestamp { get; set; }
        [JsonProperty]
        public LatLng Point { get; set; }
        [JsonProperty]
        public double? AccuracyInMetres { get; set; }
        [JsonProperty]
        public double SpeedMetresPerSecond { get; set; }
        [JsonProperty]
        public double? Altitude { get; set; }

        public double Mph => SpeedMetresPerSecond * 2.23694;

        public override string ToString() {
            return $"{Timestamp}: Lat: {Point.Latitude}, Lon: {Point.Longitude}, Accuracy: {AccuracyInMetres}, Mph: {Mph}";
        }
    }
}
