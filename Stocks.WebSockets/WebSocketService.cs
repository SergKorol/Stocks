using System.Net.WebSockets;
using System.Text.Json;
using Stocks.WebSockets.Payloads;
using Websocket.Client;

namespace Stocks.WebSockets;

public sealed class WebSocketService : IWebSocketService, IDisposable
{
    private IWebsocketClient? _webSocket;

    public void Configure(string url, string? token)
    {
        _webSocket = new WebsocketClient(new Uri($"{url}?token={token}"));
    }


    public async Task ConnectAsync()
    {
        if (_webSocket != null) await _webSocket.StartOrFail();
    }

    public void Subscribe(Action<ResponseMessage> action)
    {
        _webSocket?.MessageReceived.Subscribe(action);
    }

    public bool SendAsync(Payload payload)
    {
        return _webSocket != null && _webSocket.Send(JsonSerializer.Serialize(payload));
    }

    public void Dispose()
    {
        _webSocket?.StopOrFail(WebSocketCloseStatus.NormalClosure, "Closing WebSocket");
        _webSocket?.Dispose();
    }
}