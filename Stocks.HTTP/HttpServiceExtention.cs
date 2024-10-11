using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Domain.Contracts;
using Stocks.HTTP.Configuration;
using Stocks.HTTP.Providers;

namespace Stocks.HTTP;

public static class HttpServiceExtention
{
    public static void AddHttpServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<RequestConfiguration>(configuration.GetSection("Assets"));
        services.Configure<AuthConfiguration>(options =>
        {
            options.AuthUrl = configuration["TokenRequest:Auth"];
        });
        services.Configure<RequestConfiguration>(options =>
        {
            options.Token = configuration["AccessToken"];
        });
        services.AddHttpClient<IMarketAssetsDataProvider>();
        services.AddScoped<IMarketAssetsDataProvider, MarketAssetsDataProvider>();
        services.AddScoped<IUserProvider, UserProvider>();
    }
}