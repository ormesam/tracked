using Newtonsoft.Json;

namespace Tracked.Auth {
    [JsonObject]
    public class GoogleUser {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
