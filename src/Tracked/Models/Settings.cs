using System;
using Newtonsoft.Json;

namespace Tracked.Models {
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Settings {
        [JsonProperty]
        public Guid? Id { get; set; }
        [JsonProperty]
        public bool DetectJumps { get; set; }
        [JsonProperty]
        public Guid? BluetoothDeviceId { get; set; }

        public void ResetDefaults() {
            DetectJumps = true;
        }
    }
}
