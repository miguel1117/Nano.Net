using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nano.Net.WebSockets
{
    public class NanoWebSocketClient
    {
        public delegate void NotificationEventHandler(NanoWebSocketClient client, TopicMessage topicMessage);

        public event NotificationEventHandler Confirmation;

        private readonly ClientWebSocket _clientWebSocket = new ClientWebSocket();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Task _loop;

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
                string messageJson = Encoding.Default.GetString(content).TrimEnd((char)0);

                var confirm = JsonConvert.DeserializeObject<ConfirmationTopicMessage>(messageJson);
                confirm.OriginalJson = messageJson; // include the original json message in the message object

                Confirmation?.Invoke(this, confirm);
            }
        }

        public async Task Subscribe(Topic topic)
        {
            if (_loop.IsCompleted)
                throw new Exception("This websocket instance has been closed and therefore no subscriptions can't be made.");
            Console.WriteLine(topic.GetSubscribeCommand());
            
            await _clientWebSocket.SendAsync(Encoding.Default.GetBytes(topic.GetSubscribeCommand()), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
