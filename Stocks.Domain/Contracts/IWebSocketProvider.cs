using Stocks.Domain.Models;

namespace Stocks.Domain.Contracts;

public interface IWebSocketProvider
{
    Task<bool> ConnectionAsync(Action<UpdateWebSocketMessage> handler);
    void SubscribeMessageAsync(Guid assetId, int requestId);
}