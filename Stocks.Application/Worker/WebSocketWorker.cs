using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Stocks.Database;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;

namespace Stocks.Application.Worker;

public sealed class WebSocketWorker(IServiceScopeFactory factory)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _ = Task.Run(Worker, cancellationToken);
        await Task.Delay(Timeout.Infinite, cancellationToken);
    }

    private async Task Worker()
    {
        using var scope = factory.CreateScope();
        var webSocketProvider = scope.ServiceProvider.GetRequiredService<IWebSocketProvider>();
        var assetsRepository = scope.ServiceProvider.GetRequiredService<IMarketAssetRepository>();
        List<Guid> assetIds;
        do
        { 
            assetIds = (await assetsRepository.GetMarketAssetsIdsAsync()).ToList();
            
        } while (!assetIds.Any());
        var requestId = 0;
    
        await webSocketProvider.ConnectionAsync(UpdateMarketPriceChange(assetsRepository));
        
        foreach (var marketAssetId in assetIds)
            webSocketProvider.SubscribeMessageAsync(marketAssetId, ++requestId);
        
        await Task.Delay(Timeout.Infinite);
    }
    

    private Action<UpdateWebSocketMessage> UpdateMarketPriceChange(IMarketAssetRepository assetsRepository)
    {
        return updateWebSocketMessage => assetsRepository.UpdateMarketChangePrice(updateWebSocketMessage);
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        return base.StopAsync(stoppingToken);
    }
}


