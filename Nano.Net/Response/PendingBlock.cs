using Newtonsoft.Json;

namespace Nano.Net.Response
{
    public class PendingBlock
    {
        [JsonProperty("hash")] public string Hash { get; set; }
        [JsonProperty("amount")] public string Amount { get; set; }
        [JsonProperty("source")] public string Source { get; set; }
    }
}
