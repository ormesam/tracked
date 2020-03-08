using System;
using Newtonsoft.Json;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AccelerometerReading {
        [JsonProperty]
        public DateTime Timestamp { get; set; }

        [JsonProperty]
        public double X { get; set; }

        [JsonProperty]
        public double Y { get; set; }

        [JsonProperty]
        public double Z { get; set; }

        public TimeSpan GetTimeFromStart(DateTime start) {
            return Timestamp - start;
        }

        public override string ToString() {
            return $"{Timestamp} X: {X} Y: {Y} Z: {Z}";
        }
    }
}
