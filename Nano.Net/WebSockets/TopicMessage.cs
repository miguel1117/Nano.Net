using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public abstract class TopicMessage
    {
        [JsonIgnore] public string OriginalJson { get; set; }
        
        [JsonProperty("topic")] public string Topic { get; set; }

        [JsonProperty("time")] public string Time { get; set; }
    }
}
