using System.Threading.Tasks;
using Nano.Net.WebSockets;
using Xunit;

namespace Nano.Net.Tests
{
    public class WebSocketFixture : IAsyncLifetime
    {
        public NanoWebSocketClient Client;
        
        public async Task InitializeAsync()
        {
            Client = new NanoWebSocketClient(Constants.WebSocketAddress);
            await Client.Start();
        }

        public async Task DisposeAsync()
        {
        }
    }

    public class WebSocketsTest : IClassFixture<WebSocketFixture>
    {
        private readonly WebSocketFixture _fixture;

        public WebSocketsTest(WebSocketFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        public async void PingTest()
        {
            PingMessage ping = await _fixture.Client.Ping();
            Assert.NotNull(ping.Ack);
        }
    }
}
