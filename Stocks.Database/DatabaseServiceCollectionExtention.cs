using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stocks.Database.Mapper;
using Stocks.Database.Repositories;
using Stocks.Domain.Contracts;


namespace Stocks.Database;

public static class DatabaseServiceCollectionExtention
{
    public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        DatabaseMapConfig.ConfigureMap();
        services.AddEntityFrameworkNpgsql()
            .AddDbContext<MarketContext>(
                options => options.UseNpgsql(configuration["Postgres:DefaultConnection"]))
            .AddEntityFrameworkNpgsql();

        services.AddScoped<IMarketAssetRepository, MarketAssetRepository>();
    }
}