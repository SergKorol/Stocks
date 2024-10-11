using Mapster;
using Stocks.API.Requests;
using Stocks.API.Responses;
using Stocks.Domain.Models;

namespace Stocks.API.Map;

public static class ApiMapConfig
{
    public static void ConfigureMap()
    {
        TypeAdapterConfig<FiltersRequest, Filters>
            .NewConfig();
        
        TypeAdapterConfig<MarketAssetResponse, MarketAsset>
            .NewConfig();
    }
}