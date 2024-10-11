using Microsoft.Extensions.DependencyInjection;
using Stocks.Application.Worker;

namespace Stocks.Application;

public static class ApplicationServicesExtention
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddHostedService<WebSocketWorker>();
    }
}