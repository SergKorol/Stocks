using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocks.Database.Entities;
using Stocks.Database.Queries;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;

namespace Stocks.Database.Repositories;

public sealed class MarketAssetRepository(MarketContext marketContext, ILogger<MarketAssetRepository> logger, IServiceProvider serviceProvider) : IMarketAssetRepository
{
    public async Task<List<MarketAsset>> GetFilteredMarketAssetsAsync(Filters? filters)
    {
        var includes = new List<string> { "MarketPrice" };
        
        
        var entities = await marketContext.MarketAssets.GetQueryableAssets(includes, filters).ToListAsync();

        var assets = entities.Adapt<List<MarketAsset>>();


        return assets;
    }

    public async Task<IEnumerable<Guid>> GetMarketAssetsIdsAsync()
    {
        var ids = await marketContext.MarketAssets.AsQueryable().Select(x => x.Id).ToListAsync();
        return ids;
    }
    
    public async Task UpdateMarketAssets(IEnumerable<MarketAsset> marketAssets)
    {
        try
        {
            var assets = marketAssets.Adapt<IEnumerable<MarketAssetEntity>>();
            await marketContext.MarketAssets.AddRangeAsync(assets);
            await marketContext.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            logger.LogError("Failed to update market assets: {Message}", e.Message);
        }
    }

    public async Task UpdateMarketChangePrice(UpdateWebSocketMessage message)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var marketContext = scope.ServiceProvider.GetRequiredService<MarketContext>();

            var entity = await marketContext.MarketPrices.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == message.Id);
            if (entity == null)
            {
                MarketPriceChangeEntity newEntity;
                newEntity = message.Adapt<MarketPriceChangeEntity>();
                marketContext.MarketPrices.Add(newEntity);
            }
            else
            {
                entity = message.Adapt<MarketPriceChangeEntity>();
                marketContext.MarketPrices.Update(entity);
            }
            
            await marketContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            logger.LogError("Failed to update market change price: {Message}", ex.Message);
        }
    }
    


    public async Task<bool> IsExistsAsync()
    {
        return await marketContext.MarketAssets.AsQueryable().AnyAsync().ConfigureAwait(false);
    }
}