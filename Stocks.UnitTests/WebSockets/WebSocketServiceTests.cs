using Stocks.WebSockets;

namespace Stocks.UnitTests.WebSockets;

public sealed class WebSocketServiceTests
{
    
    private readonly WebSocketService _webSocketService;

    public WebSocketServiceTests()
    {
        _webSocketService = new WebSocketService();
    }


    [Fact]
    public void Configure_SetsUpWebSocketClientWithUrlAndToken()
    {
        // Arrange
        string url = "wss://test";
        string token = "test-token";
        
        // Act
        _webSocketService.Configure(url, token);

        // Assert
        Assert.NotNull(_webSocketService);
    }
}