using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Domain.Contracts;
using Stocks.WebSockets.Mapping;
using Stocks.WebSockets.Models;

namespace Stocks.WebSockets;

public static class WebSocketsServicesExtention
{
    public static void AddWebSocketServices(this IServiceCollection services, IConfiguration configuration)
    {
        WebSocketMapConfig.ConfigureMap();
        services.Configure<WebSocketOptions>(configuration.GetSection("WebSocket"));
        services.Configure<WebSocketOptions>(options =>
        {
            options.Token = configuration["AccessToken"];
        });
        
        services.AddHttpClient();
        services.AddTransient<IWebSocketService, WebSocketService>();
        services.AddScoped<IWebSocketProvider, WebSocketProvider>();
    }
}