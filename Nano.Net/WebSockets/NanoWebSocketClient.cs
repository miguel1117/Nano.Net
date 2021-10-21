using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Nano.Net.WebSockets
{
    public class NanoWebSocketClient
    {
        public delegate void MessageReceivedEventHandler(NanoWebSocketClient client, string content);

        public delegate void ConfirmationMessageHandler(NanoWebSocketClient client, ConfirmationTopicMessage topicTopicMessage);

        public delegate void UnconfirmedBlockMessageHandler(NanoWebSocketClient client, NewUnconfirmedBlockTopicMessage topicTopicMessage);

        /// <summary>
        /// Invoked when a message of any topic is received.
        /// </summary>
        public event MessageReceivedEventHandler Message;

        /// <summary>
        /// Invoked when a confirmation topic message is received.
        /// </summary>
        public event ConfirmationMessageHandler Confirmation;

        /// <summary>
        /// Invoked when a new unconfirmed block message is received.
        /// </summary>
        public event UnconfirmedBlockMessageHandler NewUnconfirmedBlock;

        private readonly Dictionary<string, Topic> _subscriptions = new Dictionary<string, Topic>();
        private readonly WebsocketClient _clientWebSocket;
        private TaskCompletionSource<PingMessage> _pingResponseMessage;

        /// <summary>
        /// Creates a new websocket connection to the specified node address. Needs to be started using the Start() method.
        /// Note: not all public nodes have their websockets enabled. 
        /// </summary>
        /// <param name="url">The websocket address</param>
        /// <param name="timeoutInSeconds">For how long the client waits for a message before attempting to reconnect</param>
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

            Message?.Invoke(this, messageString);

            var topic = messageJson["topic"]?.ToString();
            switch (topic)
            {
                case null:
                    throw new NanoWebSocketException("NanoWebSocketClient received unexpected message.");

                case "confirmation":
                    var confirmationMessage = JsonConvert.DeserializeObject<ConfirmationTopicMessage>(messageString);
                    Confirmation?.Invoke(this, confirmationMessage);
                    break;

                case "new_unconfirmed_block":
                    var unconfirmedBlockMessage = JsonConvert.DeserializeObject<NewUnconfirmedBlockTopicMessage>(messageString);
                    NewUnconfirmedBlock?.Invoke(this, unconfirmedBlockMessage);
                    break;
            }
        }

        private void OnReconnect()
        {
            foreach ((string _, Topic topic) in _subscriptions)
                SendSubscription(topic);
        }
        
        private void SendSubscription(Topic topic)
        {
            _clientWebSocket.Send(JsonConvert.SerializeObject(new
            {
                Action = "subscribe",
                Topic = topic.Name,
                Options = topic.GetOptions()
            }, Topic.JsonSerializerSettings));
        }

        // maybe there is a better way to do this, but i just can't imagine any right now
        public async Task<PingMessage> Ping()
        {
            _pingResponseMessage = new TaskCompletionSource<PingMessage>();
            _clientWebSocket.Send("{ \"action\": \"ping\" }");

            if (await Task.WhenAny(_pingResponseMessage.Task, Task.Delay(10000)) == _pingResponseMessage.Task)
                return _pingResponseMessage.Task.Result;
            else
                throw new NanoWebSocketException("Ping timeout exceeded.");
        }

        public void Subscribe(Topic topic)
        {
            _subscriptions[topic.Name] = topic;
            SendSubscription(topic);
        }

        public void UpdateSubscription(Topic topic)
        {
            _subscriptions[topic.Name] = topic;
            
            _clientWebSocket.Send(JsonConvert.SerializeObject(new
            {
                Action = "update",
                Topic = topic.Name,
                Options = topic.GetOptions()
            }, Topic.JsonSerializerSettings));
        }
    }
}
