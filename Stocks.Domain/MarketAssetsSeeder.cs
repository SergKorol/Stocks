using Microsoft.Extensions.DependencyInjection;
using Stocks.Domain.Contracts;

namespace Stocks.Domain;

public class MarketAssetsSeeder(IServiceScopeFactory factory) : IMarketAssetsSeeder
{
    public async Task SeedMarketAssets()
    {
        using var scope = factory.CreateScope();
        var marketAssetsRepositoryProvider = scope.ServiceProvider.GetRequiredService<IMarketAssetRepository>();
        if (!await marketAssetsRepositoryProvider.IsExistsAsync())
        {
            var assetsDataProvider = scope.ServiceProvider.GetRequiredService<IMarketAssetsDataProvider>();
            var marketAssets = await assetsDataProvider?.GetAllMarketAssetsAsync()!;

            if (marketAssets != null && marketAssets.Any())
            {
                await marketAssetsRepositoryProvider.UpdateMarketAssets(marketAssets);
            }
        }
        
    }
}