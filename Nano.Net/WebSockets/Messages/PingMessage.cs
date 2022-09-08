using Newtonsoft.Json;

namespace Nano.Net.WebSockets;

public class PingMessage
{
    public string Ack => "pong";

    [JsonProperty("time")]
    public string Time { get; set; }
}
