using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class NewUnconfirmedBlockMessage : IMessage
    {
        [JsonProperty("topic")]
        public string Topic { get; init; }
        
        [JsonProperty("time")]
        public string Time { get; init; }
        
        [JsonProperty("message")]
        public BlockWithSubtype Contents { get; set; }
    }
}
