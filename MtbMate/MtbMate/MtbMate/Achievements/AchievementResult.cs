using System;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class AchievementResult {
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public int AcheivementId { get; set; }
        [JsonProperty]
        public DateTime Time { get; set; }
        [JsonProperty]
        public Guid RideId { get; set; }
    }
}
