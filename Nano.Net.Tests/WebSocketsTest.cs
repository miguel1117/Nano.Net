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
            Client = await NanoWebSocketClient.Connect(Constants.WebSocketAddress);
        }

        public async Task DisposeAsync()
        {
            await Client.Stop();
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
            Assert.NotNull(ping.Time);
        }
    }
}
