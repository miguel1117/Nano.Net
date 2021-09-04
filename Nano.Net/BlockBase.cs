using Newtonsoft.Json;

namespace Nano.Net
{
    public class BlockBase
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "state";

        [JsonProperty("account")]
        public string Account { get; init; }

        [JsonProperty("previous")]
        public string Previous { get; init; }

        [JsonProperty("representative")]
        public string Representative { get; init; }

        [JsonProperty("balance")]
        public string Balance { get; init; }

        [JsonProperty("link")]
        public string Link { get; init; }
        [JsonProperty("link_as_account")]
        public string LinkAsAccount { get; init; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("work")]
        public string Work { get; set; }
    }
}
