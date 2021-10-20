using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

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

        private readonly List<Topic> _subscriptions = new List<Topic>();
        private readonly WebsocketClient _clientWebSocket;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private TaskCompletionSource<PingMessage> _pingResponseMessage;

        public NanoWebSocketClient(string url, double timeoutInSeconds = 20D)
        {
            _clientWebSocket = new WebsocketClient(new Uri(url));
            
            _clientWebSocket.ReconnectTimeout = TimeSpan.FromSeconds(timeoutInSeconds);
            _clientWebSocket.MessageReceived.Subscribe(c => OnMessage(c));
            _clientWebSocket.ReconnectionHappened.Subscribe(_ => OnReconnect());
        }

        public async Task Start()
        {
            await _clientWebSocket.Start();
        }

        private void OnMessage(ResponseMessage message)
        {
            var messageString = message.ToString();
            var messageJson = JObject.Parse(messageString);

            // listen for ping responses
            if (_pingResponseMessage is not null && messageJson["ack"]?.ToString() == "pong")
            {
                _pingResponseMessage.TrySetResult(JsonConvert.DeserializeObject<PingMessage>(messageString));
                return;
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

        private void OnReconnect()
        {
            foreach (Topic subscription in _subscriptions)
                _clientWebSocket.Send(subscription.GetSubscribeCommand());
        }
        
        // maybe there is a better way to do this, but i just can't imagine any right now
        public async Task<PingMessage> Ping()
        {
            const string ping = "{ \"action\": \"ping\" }";
        
            _pingResponseMessage = new TaskCompletionSource<PingMessage>();
            _clientWebSocket.Send(ping);
        
            if (await Task.WhenAny(_pingResponseMessage.Task, Task.Delay(10000)) == _pingResponseMessage.Task)
                return _pingResponseMessage.Task.Result;
            else
                throw new NanoWebSocketException("Ping timeout exceeded.");
        }

        public void Subscribe(Topic topic)
        {
            _subscriptions.Add(topic);
            _clientWebSocket.Send(topic.GetSubscribeCommand());
        }
    }
}
