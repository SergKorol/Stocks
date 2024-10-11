using Stocks.Domain.Contracts;

namespace Stocks.API.Middleware;

public sealed class MarketAssetsMiddleware
{
    private readonly RequestDelegate _next;
    private static bool _hasRun = false;

    public MarketAssetsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider services)
    {
        var configuration = services.GetRequiredService<IConfiguration>();
        var userProvider = services.GetRequiredService<IUserProvider>();
        if (!_hasRun)
        {
            _hasRun = true;
            await userProvider.SetUserToken(configuration["User:UserName"],
                configuration["User:Password"]);
            var assetsSeeder = services.GetRequiredService<IMarketAssetsSeeder>();
            assetsSeeder?.SeedMarketAssets();
        }
        else
        {
            await userProvider.SetUserToken(configuration["User:UserName"],
                configuration["User:Password"]);
        }
        

        await _next(context);
    }
}



