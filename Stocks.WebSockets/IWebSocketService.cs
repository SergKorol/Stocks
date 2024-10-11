using Stocks.WebSockets.Payloads;
using Websocket.Client;

namespace Stocks.WebSockets;

public interface IWebSocketService
{
    Task ConnectAsync();

    void Configure(string url, string? token);

    void Subscribe(Action<ResponseMessage> action);

    bool SendAsync(Payload payload);
}