using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Jump {
        [JsonProperty]
        public int Number { get; set; }
        [JsonProperty]
        public DateTime Timestamp { get; set; }
        [JsonProperty]
        public double Airtime { get; set; }
        [JsonProperty]
        public IList<AccelerometerReading> Readings { get; set; }

        public override string ToString() {
            return $"Jump {Number} - Time: {Timestamp}, Airtime: {Airtime}s";
        }
    }
}
