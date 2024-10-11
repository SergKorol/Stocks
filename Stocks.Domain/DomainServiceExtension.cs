using Microsoft.Extensions.DependencyInjection;
using Stocks.Domain.Contracts;

namespace Stocks.Domain;

public static class DomainServiceExtension
{
    public static void AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IMarketAssetsSeeder, MarketAssetsSeeder>();
    }
}