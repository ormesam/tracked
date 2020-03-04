using System;
using Newtonsoft.Json;

namespace MtbMate.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Settings {
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public bool DetectJumps { get; set; }

        public void ResetDefaults() {
            DetectJumps = true;
        }
    }
}
