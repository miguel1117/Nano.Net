using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class NewUnconfirmedBlockMessage : Message
    {
        [JsonProperty("message")]
        public BlockWithSubtype Contents { get; set; }
    }
}
