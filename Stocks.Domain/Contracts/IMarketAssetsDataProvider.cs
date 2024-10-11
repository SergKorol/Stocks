using Stocks.Domain.Models;

namespace Stocks.Domain.Contracts;

public interface IMarketAssetsDataProvider
{
    Task<IEnumerable<MarketAsset>?> GetAllMarketAssetsAsync();
}