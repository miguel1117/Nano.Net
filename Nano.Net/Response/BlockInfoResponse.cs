using Newtonsoft.Json;

namespace Nano.Net.Response
{
    public class BlockInfoResponse
    {
        [JsonProperty("block_account")] public string BlockAccount { get; init; }
        [JsonProperty("amount")] public string Amount { get; init; }
        [JsonProperty("height")] public string Height { get; init; }
        [JsonProperty("confirmed")] public bool Confirmed { get; init; }
        [JsonProperty("contents")] public BlockBase Contents { get; init; }
        [JsonProperty("subtype")] public string Subtype { get; init; }
    }
}
