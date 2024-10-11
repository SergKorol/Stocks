using Microsoft.Extensions.Logging;
using Stocks.Database;
using Stocks.Database.Entities;
using Stocks.Database.Repositories;
using Stocks.Domain.Models;
using Stocks.UnitTests.Fixture;

namespace Stocks.UnitTests.Database;

public sealed class MarketAssetRepositoryTests : DatabaseFixture
{
    private readonly MarketContext _marketContext;
    private readonly MarketAssetRepository _marketAssetRepository;

    public MarketAssetRepositoryTests()
    {
        var fixture = new DatabaseFixture();
        _marketContext = fixture.CreateContext();
        ILogger<MarketAssetRepository> logger = fixture.Logger;
        var serviceScopeFactory = fixture.ServiceScopeFactory;
        _marketAssetRepository = new MarketAssetRepository(_marketContext, logger, serviceScopeFactory);
    }

    [Fact]
    public async Task GetFilteredMarketAssetsAsync_ShouldReturnAssets()
    {
        // Arrange
        var filters = new Filters();
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var marketAssets = new List<MarketAssetEntity>
        {
            new MarketAssetEntity 
            { 
                Id = id1, 
                Description = "Asset 1",
                Kind = "Equity",
                Symbol = "AAPL",
                MarketPrice = new MarketPriceChangeEntity { Id = id1, AskPrice = 1}
            },
            new MarketAssetEntity 
            { 
                Id = id2, 
                Description = "Asset 2",
                Kind = "Equity",
                Symbol = "MSFT",
                MarketPrice = new MarketPriceChangeEntity { Id = id2, AskPrice = 2}
            }
        };

        _marketContext.MarketAssets.AddRange(marketAssets);
        await _marketContext.SaveChangesAsync();

        // Act
        var result = await _marketAssetRepository.GetFilteredMarketAssetsAsync(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Asset 1", result[0].Description);
    }
    
    [Fact]
    public async Task GetFilteredMarketAssetsAsync_ShouldReturnFilteredAssets()
    {
        // Arrange
        var filters = new Filters { Kind = "Equity" };
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var marketAssets = new List<MarketAssetEntity>
        {
            new()
            { 
                Id = id1, 
                Description = "Asset 1",
                Kind = "Equity",
                Symbol = "AAPL",
                MarketPrice = new MarketPriceChangeEntity { Id = id1, AskPrice = 1 }
            },
            new()
            { 
                Id = id2, 
                Description = "Asset 2",
                Kind = "Commodity",
                Symbol = "OIL",
                MarketPrice = new MarketPriceChangeEntity { Id = id2, AskPrice = 2 }
            }
        };

        _marketContext.MarketAssets.AddRange(marketAssets);
        await _marketContext.SaveChangesAsync();

        // Act
        var result = await _marketAssetRepository.GetFilteredMarketAssetsAsync(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Asset 1", result[0].Description);
    }
    
    [Fact]
    public async Task GetFilteredMarketAssetsAsync_ShouldReturnEmptyList_WhenNoMatchingAssets()
    {
        // Arrange
        var filters = new Filters { Symbol = "XYZ" };
        var id1 = Guid.NewGuid();
        var marketAssets = new List<MarketAssetEntity>
        {
            new()
            { 
                Id = id1, 
                Description = "Asset 1",
                Kind = "Equity",
                Symbol = "AAPL",
                MarketPrice = new MarketPriceChangeEntity { Id = id1, AskPrice = 1 }
            }
        };

        _marketContext.MarketAssets.AddRange(marketAssets);
        await _marketContext.SaveChangesAsync();

        // Act
        var result = await _marketAssetRepository.GetFilteredMarketAssetsAsync(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task GetFilteredMarketAssetsAsync_ShouldApplyMultipleFilters()
    {
        // Arrange
        var filters = new Filters { Kind = "Equity", Symbol = "AAPL" };
        var id1 = Guid.NewGuid();
        var id2 = Guid.NewGuid();
        var marketAssets = new List<MarketAssetEntity>
        {
            new()
            { 
                Id = id1, 
                Description = "Asset 1",
                Kind = "Equity",
                Symbol = "AAPL",
                MarketPrice = new MarketPriceChangeEntity { Id = id1, AskPrice = 1 }
            },
            new()
            { 
                Id = id2, 
                Description = "Asset 2",
                Kind = "Equity",
                Symbol = "MSFT",
                MarketPrice = new MarketPriceChangeEntity { Id = id2, AskPrice = 2 }
            }
        };

        _marketContext.MarketAssets.AddRange(marketAssets);
        await _marketContext.SaveChangesAsync();

        // Act
        var result = await _marketAssetRepository.GetFilteredMarketAssetsAsync(filters);

        // Assert
        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Asset 1", result[0].Description);
    }
}