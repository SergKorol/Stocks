using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Stocks.Database;

public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MarketContext>
{
    public MarketContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory().Replace("Database", "API"))
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var connectionString = configuration["Postgres:DefaultConnection"];
        
        var builder = new DbContextOptionsBuilder<MarketContext>();
        builder.UseNpgsql(connectionString);
    
        return new MarketContext(builder.Options);
    }
}