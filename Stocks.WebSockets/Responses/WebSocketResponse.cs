using Stocks.Domain.Models;

namespace Stocks.WebSockets.Responses;

public record WebSocketResponse
{
    public Guid? InstrumentId { get; init; }
    public MarketPriceChange? Ask { get; init; }
    public MarketPriceChange? Bid { get; init; }
    public MarketPriceChange? Last { get; init; }
    
    
}