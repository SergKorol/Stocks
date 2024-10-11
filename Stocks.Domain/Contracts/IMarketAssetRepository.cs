using Stocks.Domain.Models;

namespace Stocks.Domain.Contracts;

public interface IMarketAssetRepository
{
    Task<List<MarketAsset>> GetFilteredMarketAssetsAsync(Filters? filters);

    Task<IEnumerable<Guid>> GetMarketAssetsIdsAsync();

    Task UpdateMarketChangePrice(UpdateWebSocketMessage message);

    Task UpdateMarketAssets(IEnumerable<MarketAsset> marketAssets);

    Task<bool> IsExistsAsync();
}