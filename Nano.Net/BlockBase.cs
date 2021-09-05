using Newtonsoft.Json;

namespace Nano.Net
{
    public class BlockBase
    {
        [JsonProperty("type")]
        public string Type => "state";

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

        [JsonProperty("signature")]
        public string Signature { get; set; }

        [JsonProperty("work")]
        public string Work { get; set; }
    }


    public class BlockWithSubtype : BlockBase
    {
        // not all blocks returned from responses contain the subtype inside the block
        [JsonProperty("subtype")]
        public string Subtype { get; init; }
    }
}
