using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Stocks.Database;
using Stocks.Database.Repositories;

namespace Stocks.UnitTests.Fixture;

public class DatabaseFixture : IDisposable
{
    public MarketContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MarketContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new MarketContext(options);
    }

    public ILogger<MarketAssetRepository> Logger { get; }
    public IServiceProvider ServiceScopeFactory { get; }

    protected internal DatabaseFixture()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();
        var scope = app.Services.CreateScope();
        ServiceScopeFactory = scope.ServiceProvider;

        var loggerFactory = new LoggerFactory();
        Logger = new Logger<MarketAssetRepository>(loggerFactory);
    }

    public void Dispose()
    {
    }
}