using Mapster;
using Stocks.Domain.Models;
using Stocks.WebSockets.Responses;

namespace Stocks.WebSockets.Mapping;

public static class WebSocketMapConfig
{
    public static void ConfigureMap()
    {
        TypeAdapterConfig<WebSocketResponse, UpdateWebSocketMessage>
            .NewConfig()
            .Map(dest => dest.Id, src => src.InstrumentId)
            .Map(dest => dest.AskPrice, src => src.Ask!.Price)
            .Map(dest => dest.BidPrice, src => src.Bid!.Price)
            .Map(dest => dest.LastPrice, src => src.Last!.Price)
            .Map(dest => dest.AskUpdated, src => src.Ask!.TimeStamp)
            .Map(dest => dest.BidUpdated, src => src.Bid!.TimeStamp)
            .Map(dest => dest.LastUpdated, src => src.Last!.TimeStamp);
    }
}