using Newtonsoft.Json;

namespace Nano.Net.Response
{
    public class PendingBlocksResponse
    {
        [JsonProperty("blocks")] public string[] Blocks { get; init; }
    }
}
