using System;
using System.Threading.Tasks;
using Nano.Net.WebSockets;
using Xunit;

namespace Nano.Net.Tests;

public class WebSocketFixture : IAsyncLifetime
{
    public readonly NanoWebSocketClient Client = new NanoWebSocketClient(Constants.WebSocketAddress);

    public async Task InitializeAsync()
    {
        await Client.Start().ConfigureAwait(false);
        Console.WriteLine();
    }

    public async Task DisposeAsync()
    {
        await Client.Stop().ConfigureAwait(false);
        Client.Dispose();
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
    public async void WebSocketPing_CheckResponse_IsNotNull()
    {
        PingMessage ping = await _fixture.Client.Ping().ConfigureAwait(false);

        Assert.NotNull(ping.Ack);
    }
}
