using Newtonsoft.Json;

namespace Nano.Net.WebSockets;

public interface ITopicMessage
{
    public string Topic { get; init; }
    public string Time { get; init; }
}
