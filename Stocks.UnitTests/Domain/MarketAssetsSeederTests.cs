using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stocks.Domain;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;

namespace Stocks.UnitTests.Domain;

public sealed class MarketAssetsSeederTests
{
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
    private readonly Mock<IServiceScope> _serviceScopeMock;
    private readonly Mock<IMarketAssetRepository> _marketAssetRepositoryMock;
    private readonly Mock<IMarketAssetsDataProvider> _marketAssetsDataProviderMock;
    private readonly MarketAssetsSeeder _marketAssetsSeeder;

    public MarketAssetsSeederTests()
    {
        _serviceProviderMock = new Mock<IServiceProvider>();
        _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
        _serviceScopeMock = new Mock<IServiceScope>();
        _marketAssetRepositoryMock = new Mock<IMarketAssetRepository>();
        _marketAssetsDataProviderMock = new Mock<IMarketAssetsDataProvider>();

        AssertMockNotNull(_serviceProviderMock, nameof(_serviceProviderMock));
        AssertMockNotNull(_serviceScopeFactoryMock, nameof(_serviceScopeFactoryMock));
        AssertMockNotNull(_serviceScopeMock, nameof(_serviceScopeMock));
        AssertMockNotNull(_marketAssetRepositoryMock, nameof(_marketAssetRepositoryMock));
        AssertMockNotNull(_marketAssetsDataProviderMock, nameof(_marketAssetsDataProviderMock));

        _serviceScopeFactoryMock.Setup(factory => factory.CreateScope()).Returns(_serviceScopeMock.Object);

        _serviceScopeMock.Setup(scope => scope.ServiceProvider.GetService(typeof(IMarketAssetRepository)))
            .Returns(_marketAssetRepositoryMock.Object);
        _serviceScopeMock.Setup(scope => scope.ServiceProvider.GetService(typeof(IMarketAssetsDataProvider)))
            .Returns(_marketAssetsDataProviderMock.Object);

        _marketAssetsSeeder = new MarketAssetsSeeder(_serviceScopeFactoryMock.Object);
        
        if (_marketAssetsSeeder == null)
            throw new NullReferenceException("MarketAssetsSeeder instance is not initialized.");
    }

    private void AssertMockNotNull(object mockObject, string mockName)
    {
        if (mockObject == null)
            throw new NullReferenceException($"Mock for {mockName} is not initialized.");
    }

    [Fact]
    public async Task SeedMarketAssets_ShouldNotUpdate_WhenAssetsExist()
    {
        // Arrange
        _marketAssetRepositoryMock.Setup(repo => repo.IsExistsAsync()).ReturnsAsync(true);

        // Act
        await _marketAssetsSeeder.SeedMarketAssets();

        // Assert
        _marketAssetRepositoryMock
            .Verify(repo => repo.UpdateMarketAssets(It.IsAny<IEnumerable<MarketAsset>>()), Times.Never);
    }

    [Fact]
    public async Task SeedMarketAssets_ShouldUpdate_WhenAssetsDoNotExist()
    {
        _marketAssetRepositoryMock.Setup(repo => repo.IsExistsAsync()).ReturnsAsync(false);

        var marketAssets = new List<MarketAsset>
        {
            new MarketAsset { Id = Guid.NewGuid(), Description = "Asset1" },
            new MarketAsset { Id = Guid.NewGuid(), Description = "Asset2" }
        };

        _marketAssetsDataProviderMock
            .Setup(provider => provider.GetAllMarketAssetsAsync())
            .ReturnsAsync(marketAssets);

        await _marketAssetsSeeder.SeedMarketAssets();

        _marketAssetRepositoryMock
            .Verify(repo => repo.UpdateMarketAssets(marketAssets), Times.Once);
    }

    [Fact]
    public async Task SeedMarketAssets_ShouldNotUpdate_WhenNoAssetsReturned()
    {
        _marketAssetRepositoryMock.Setup(repo => repo.IsExistsAsync()).ReturnsAsync(false);

        _marketAssetsDataProviderMock
            .Setup(provider => provider.GetAllMarketAssetsAsync())
            .ReturnsAsync(new List<MarketAsset>());

        await _marketAssetsSeeder.SeedMarketAssets();

        _marketAssetRepositoryMock
            .Verify(repo => repo.UpdateMarketAssets(It.IsAny<IEnumerable<MarketAsset>>()), Times.Never);
    }
}