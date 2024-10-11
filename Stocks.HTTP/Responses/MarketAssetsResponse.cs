using Stocks.Domain.Models;

namespace Stocks.HTTP.Responses;

public record MarketAssetsResponse
{
    public Paging Paging { get; set; }
    public IEnumerable<MarketAsset> Data { get; set; }
}