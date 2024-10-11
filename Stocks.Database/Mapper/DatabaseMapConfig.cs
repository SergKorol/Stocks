using Mapster;
using Stocks.Database.Entities;
using Stocks.Domain.Models;

namespace Stocks.Database.Mapper;

public static class DatabaseMapConfig
{
    public static void ConfigureMap()
    {
        TypeAdapterConfig<MarketAsset, MarketAssetEntity>
            .NewConfig()
            .Map(dest => dest.Id, src => src.Id)
            .Map(dest => dest.Symbol, src => src.Symbol)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.Kind, src => src.Kind)
            .Map(dest => dest.Currency, src => src.Currency)
            .Map(dest => dest.Exchange, src => src.Exchange)
            .Map(dest => dest.TickSize, src => src.TickSize);
            // .Map(dest => dest.MarketPrice!.Id, src => src.Id)
            // .Map(dest => dest.MarketPrice.AskPrice, src => src.AskPrice)
            // .Map(dest => dest.MarketPrice.BidPrice, src => src.BidPrice)
            // .Map(dest => dest.MarketPrice.LastPrice, src => src.LastPrice)
            // .Map(dest => dest.MarketPrice.AskUpdated, src => src.AskUpdated ?? null)
            // .Map(dest => dest.MarketPrice.BidUpdated, src => src.BidUpdated ?? null)
            // .Map(dest => dest.MarketPrice.LastUpdated, src => src.LastUpdated ?? null);

       TypeAdapterConfig<MarketAssetEntity, MarketAsset>
           .NewConfig()
           .Map(dest => dest.Id, src => src.Id)
           .Map(dest => dest.Symbol, src => src.Symbol)
           .Map(dest => dest.Description, src => src.Description)
           .Map(dest => dest.Kind, src => src.Kind)
           .Map(dest => dest.Currency, src => src.Currency)
           .Map(dest => dest.Exchange, src => src.Exchange)
           .Map(dest => dest.TickSize, src => src.TickSize)
           .Map(dest => dest.AskPrice, src => src.MarketPrice != null ? src.MarketPrice.AskPrice : default)
           .Map(dest => dest.BidPrice, src => src.MarketPrice != null ? src.MarketPrice.BidPrice : default)
           .Map(dest => dest.LastPrice, src => src.MarketPrice != null ? src.MarketPrice.LastPrice : default)
           .Map(dest => dest.AskUpdated, src => src.MarketPrice != null ? src.MarketPrice.AskUpdated : null)
           .Map(dest => dest.BidUpdated, src => src.MarketPrice != null ? src.MarketPrice.BidUpdated : null)
           .Map(dest => dest.LastUpdated, src => src.MarketPrice != null ? src.MarketPrice.LastUpdated : null);

       TypeAdapterConfig<UpdateWebSocketMessage, MarketPriceChangeEntity>
           .NewConfig()
           .Map(dest => dest.Id, src => src.Id)
           .Map(dest => dest.AskPrice, src => src.AskPrice)
           .Map(dest => dest.AskUpdated,
               src => src.AskUpdated != null
                   ? DateTime.SpecifyKind((DateTime)src.AskUpdated,
                       DateTimeKind.Utc)
                   : src.AskUpdated)
           .Map(dest => dest.BidPrice, src => src.BidPrice)
           .Map(dest => dest.BidUpdated,
               src => src.BidUpdated != null
                   ? DateTime.SpecifyKind((DateTime)src.BidUpdated,
                       DateTimeKind.Utc)
                   : src.BidUpdated)
           .Map(dest => dest.LastPrice, src => src.LastPrice)
           .Map(dest => dest.LastUpdated,
               src => src.LastUpdated != null
                   ? DateTime.SpecifyKind((DateTime)src.LastUpdated,
                       DateTimeKind.Utc)
                   : src.LastUpdated);


    }
}