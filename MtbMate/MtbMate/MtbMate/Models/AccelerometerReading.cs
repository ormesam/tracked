using System;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AccelerometerReading {
        [JsonProperty]
        public DateTime Timestamp { get; set; }

        [JsonProperty]
        public double Value { get; set; }

        [JsonProperty]
        public double SmoothedValue { get; set; }

        public override string ToString() {
            return $"{Timestamp} Reading: {Value}";
        }
    }
}
