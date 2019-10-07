using System;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Jump {
        [JsonProperty]
        public int Number { get; set; }
        [JsonProperty]
        public DateTime Time { get; set; }
        [JsonProperty]
        public double Airtime { get; set; }
        [JsonProperty]
        public double LandingGForce { get; set; }
    }
}
