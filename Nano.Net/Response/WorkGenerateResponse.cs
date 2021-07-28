using Newtonsoft.Json;

namespace Nano.Net
{
    public class WorkGenerateResponse
    {
        [JsonProperty("work")] public string Work { get; init; }
        [JsonProperty("difficulty")] public string Difficulty { get; init; }
        [JsonProperty("multiplier")] public string Multiplier { get; init; }
        [JsonProperty("hash")] public string Hash { get; init; }
    }
}
