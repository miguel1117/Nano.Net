using Newtonsoft.Json;

namespace Nano.Net;

public class ProcessResponse
{
    [JsonProperty("hash")] public string Hash { get; init; }
}
