using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public interface IMessage
    {
        public string Topic { get; init; }
        public string Time { get; init; }
    }
}
