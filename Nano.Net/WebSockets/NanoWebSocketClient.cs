using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nano.Net.WebSockets
{
    public class NanoWebSocketClient
    {
        public delegate void MessageReceivedEventHandler(NanoWebSocketClient client, string topic, ITopicMessage topicMessage);
        public delegate void ConfirmationMessageHandler(NanoWebSocketClient client, ConfirmationTopicMessage topicTopicMessage);
        public delegate void UnconfirmedBlockMessageHandler(NanoWebSocketClient client, NewUnconfirmedBlockTopicMessage topicTopicMessage);
        
        public event MessageReceivedEventHandler NewMessage;
        public event ConfirmationMessageHandler Confirmation;
        public event UnconfirmedBlockMessageHandler NewUnconfirmedBlock;

        private readonly ClientWebSocket _clientWebSocket = new ClientWebSocket();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _loop;
        private TaskCompletionSource<PingMessage> _ping;

        private NanoWebSocketClient()
        {
        }

        public static async Task<NanoWebSocketClient> Connect(string url)
        {
            var webSocket = new NanoWebSocketClient();
            await webSocket._clientWebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
            webSocket._loop = webSocket.Loop();

            return webSocket;
        }

        public async Task Stop()
        {
            _cancellationTokenSource.Cancel();
            await _loop; // wait for loop to close
            _clientWebSocket.Abort();
            _clientWebSocket.Dispose();
        }

        private async Task Loop()
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                var content = new byte[2048];
                try
                {
                    await _clientWebSocket.ReceiveAsync(content, _cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                    return;
                }

                string messageString = Encoding.Default.GetString(content).TrimEnd((char)0);
                var messageJson = JObject.Parse(messageString);
                
                // listen for ping responses
                if (_ping is not null && messageJson["ack"]?.ToString() == "pong")
                {
                    _ping.TrySetResult(JsonConvert.DeserializeObject<PingMessage>(messageString));
                    continue;
                }
                    
                var topic = messageJson["topic"]?.ToString();
                switch (topic)
                {
                    case null:
                        throw new NanoWebSocketException("NanoWebSocketClient received unexpected message.");
                    
                    case "confirmation":
                        var confirmationMessage = JsonConvert.DeserializeObject<ConfirmationTopicMessage>(messageString);
                        NewMessage?.Invoke(this, topic, confirmationMessage);
                        Confirmation?.Invoke(this, confirmationMessage);
                        break;
                    
                    case "new_unconfirmed_block":
                        var unconfirmedBlockMessage = JsonConvert.DeserializeObject<NewUnconfirmedBlockTopicMessage>(messageString);
                        NewMessage?.Invoke(this, topic, unconfirmedBlockMessage);
                        NewUnconfirmedBlock?.Invoke(this, unconfirmedBlockMessage);
                        break;
                }
            }
        }

        public async Task<PingMessage> Ping()
        {
            const string ping = "{ \"action\": \"ping\" }";
            
            _ping = new TaskCompletionSource<PingMessage>();
            await _clientWebSocket.SendAsync(Encoding.Default.GetBytes(ping), WebSocketMessageType.Text, true, CancellationToken.None);

            if (await Task.WhenAny(_ping.Task, Task.Delay(10000)) == _ping.Task)
                return _ping.Task.Result;
            else
                throw new NanoWebSocketException("Ping timeout exceeded.");
        }

        public async Task Subscribe(Topic topic)
        {
            if (_loop.IsCompleted)
                throw new Exception("This websocket instance has been closed and therefore no subscriptions can't be made.");

            await _clientWebSocket.SendAsync(Encoding.Default.GetBytes(topic.GetSubscribeCommand()), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
