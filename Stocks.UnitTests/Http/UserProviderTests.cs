using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using Stocks.Domain.Models;
using Stocks.HTTP.Configuration;
using Stocks.HTTP.Providers;

namespace Stocks.UnitTests.Http;

public sealed class UserProviderTests
{
    private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
    private readonly Mock<IOptions<AuthConfiguration>> _authOptionsMock;
    private readonly Mock<ILogger<UserProvider>> _loggerMock;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly UserProvider _userProvider;
    private readonly List<string> _logMessages;

    public UserProviderTests()
    {
        _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        _authOptionsMock = new Mock<IOptions<AuthConfiguration>>();
        _loggerMock = new Mock<ILogger<UserProvider>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);

        _authOptionsMock.Setup(options => options.Value).Returns(new AuthConfiguration { AuthUrl = "https://fakeauthurl.com" });

        _logMessages = new List<string>();
        _loggerMock.Setup(log => log.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>())
        ).Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((level, eventId, state, exception, formatter) =>
        {
            var message = formatter(state, exception);
            _logMessages.Add(message);
        });

        _userProvider = new UserProvider(_httpClientFactoryMock.Object, _authOptionsMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task SetUserToken_ShouldSaveTokenToAppSettings_OnSuccess()
    {
        // Arrange
        var username = "testuser";
        var password = "testpassword";
        var fakeToken = new TokenInfo { AccessToken = "fakeAccessToken" };
        var jsonResponse = JsonSerializer.Serialize(fakeToken);

        var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(jsonResponse)
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
            .ReturnsAsync(httpResponseMessage);

        // Act
        await _userProvider.SetUserToken(username, password);

        // Assert
        Assert.DoesNotContain("Request failed", _logMessages);
    }
}