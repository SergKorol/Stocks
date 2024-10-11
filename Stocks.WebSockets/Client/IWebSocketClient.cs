using System.Net.WebSockets;
using Websocket.Client;

namespace Stocks.WebSockets.Client;

public interface IWebSocketClient : IDisposable
{
    Task StartOrFail();
    IObservable<ResponseMessage> MessageReceived { get; }
    bool Send(string message);
    void StopOrFail(WebSocketCloseStatus closeStatus, string statusDescription);
}