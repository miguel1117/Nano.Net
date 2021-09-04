using Newtonsoft.Json;

namespace Nano.Net
{
    public class BlockInfoResponse
    {
        [JsonProperty("block_account")]
        public string BlockAccount { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
        [JsonProperty("balance")]
        public string Balance { get; set; }
        [JsonProperty("height")]
        public ulong Height { get; set; }
        [JsonProperty("local_timestamp")]
        public ulong LocalTimestamp { get; set; }
        [JsonProperty("confirmed")]
        public bool Confirmed { get; set; }
        [JsonProperty("content")]
        public BlockInfoResponseContent Content { get; set; }
        [JsonProperty("subtype")]
        public string Subtype { get; set; }
    }

    public class BlockInfoResponseContent
    {
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("previous")]
        public string Previous { get; set; }
        [JsonProperty("representative")]
        public string Representative { get; set; }
        [JsonProperty("balance")]
        public string Balance { get; set; }
        [JsonProperty("link")]
        public string Link { get; set; }
        [JsonProperty("link_as_account")]
        public string LinkAsAccount { get; set; }
        [JsonProperty("signature")]
        public string Signature { get; set; }
        [JsonProperty("work")]
        public string Work { get; set; }
    }
}