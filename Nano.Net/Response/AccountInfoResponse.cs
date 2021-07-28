using Newtonsoft.Json;

namespace Nano.Net
{
    public class AccountInfoResponse
    {
        [JsonProperty("frontier")] public string Frontier { get; init; }

        [JsonProperty("balance")] public string Balance { get; init; }

        [JsonProperty("block_count")] public string BlockCount { get; init; }

        [JsonProperty("representative")] public string Representative { get; init; }
    }
}
