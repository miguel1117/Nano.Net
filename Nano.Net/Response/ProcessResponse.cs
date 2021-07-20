using Newtonsoft.Json;

namespace Nano.Net.Response
{
    public class ProcessResponse
    {
        [JsonProperty("hash")] public string Hash { get; init; } 
    }
}
