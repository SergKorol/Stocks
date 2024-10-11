using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Stocks.Domain.Models;
using Stocks.HTTP.Configuration;
using Stocks.HTTP.Providers;
using Stocks.HTTP.Responses;

namespace Stocks.UnitTests.Http;

public sealed class MarketAssetsDataProviderTests
{
    private readonly Mock<IOptions<RequestConfiguration>> _optionsMock;
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly HttpClient _httpClient;
    private readonly MarketAssetsDataProvider _dataProvider;

    public MarketAssetsDataProviderTests()
    {
        _optionsMock = new Mock<IOptions<RequestConfiguration>>();

        _optionsMock.Setup(o => o.Value).Returns(new RequestConfiguration
        {
            BaseUrl = "https://api.example.com",
            Token = "test-token"
        });

        _httpClientFactoryMock = new Mock<IHttpClientFactory>();

        _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        _httpClient = new HttpClient(_httpMessageHandlerMock.Object);

        _httpClientFactoryMock.Setup(f => f.CreateClient("Instruments")).Returns(_httpClient);

        _dataProvider = new MarketAssetsDataProvider(_httpClientFactoryMock.Object, _optionsMock.Object);
    }

    [Fact]
    public async Task GetAllMarketAssetsAsync_ReturnsAssets_OnSuccess()
    {
        // Arrange
        var mockResponse = new MarketAssetsResponse
        {
            Data = new List<MarketAsset>
            {
                new() { Id = Guid.NewGuid(), Description = "Asset 1" },
                new() { Id = Guid.NewGuid(), Description = "Asset 2" }
            }
        };
        var jsonResponse = JsonSerializer.Serialize(mockResponse);
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _dataProvider.GetAllMarketAssetsAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result?.Count());
        Assert.Equal("Asset 1", result.ToList()[0].Description);
    }

    [Fact]
    public async Task GetAllMarketAssetsAsync_ReturnsNull_OnInvalidResponse()
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Invalid content")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _dataProvider.GetAllMarketAssetsAsync();

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllMarketAssetsAsync_ReturnsNull_OnErrorResponse()
    {
        // Arrange
        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("")
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        var result = await _dataProvider.GetAllMarketAssetsAsync();

        // Assert
        Assert.Null(result);
    }
}