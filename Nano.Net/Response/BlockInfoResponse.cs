using Newtonsoft.Json;

namespace Nano.Net;

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
    [JsonProperty("contents")]
    public BlockBase Content { get; set; }
    [JsonProperty("subtype")]
    public string Subtype { get; set; }
}
