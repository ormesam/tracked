using Newtonsoft.Json;

namespace MtbMate.Auth {
    [JsonObject]
    public class GoogleUser {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
