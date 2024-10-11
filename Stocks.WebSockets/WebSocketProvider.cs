using System.Net.WebSockets;
using System.Text.Json;
using Mapster;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;
using Stocks.WebSockets.Models;
using Stocks.WebSockets.Payloads;
using Stocks.WebSockets.Responses;

namespace Stocks.WebSockets;

public sealed class WebSocketProvider(
    IWebSocketService webSocketService,
    ILogger<WebSocketProvider> logger,
    IOptions<WebSocketOptions> options)
    : IWebSocketProvider
{
    public async Task<bool> ConnectionAsync(Action<UpdateWebSocketMessage> handler)
    {
        webSocketService.Configure(options.Value.BaseUrl, options.Value.Token);
        webSocketService.Subscribe(x =>
            {
                try
                {
                    if (x.Text == null) return;
                    var response = JsonSerializer.Deserialize<WebSocketResponse>(x.Text,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (response?.InstrumentId is not null)
                    {
                        handler(response.Adapt<UpdateWebSocketMessage>());
                    }
                }
                catch (JsonException e)
                {
                    logger.LogError(e, "Failed to parse response {Message} {StackTrace}", e.Message, e.StackTrace);
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Failed to parse response {Message} {StackTrace}", e.Message, e.StackTrace);
                }
            }
            );
        
        try
        {
            await webSocketService.ConnectAsync();
        }
        catch (WebSocketException ex)
        {
            logger.LogError("Failed WebSocketServer: {Message}, stack trace: {StackTrace}", ex.Message, ex.StackTrace);
            return false;
        }
        catch (Exception ex)
        {
            logger.LogWarning("Inner exception: {Message}, stack trace: {StackTrace}", ex.Message, ex.StackTrace);
            return false;
        }
        
        return true;
    }

    public void SubscribeMessageAsync(Guid assetId, int requestId)
    {
        var request = new Payload
        {
            Type = "l1-subscription",
            Id = requestId.ToString(),
            InstrumentId = assetId.ToString(),
            Provider = "simulation",
            Subscribe = true,
            Kinds = [ "ask", "bid", "last" ]
        };

        try
        {
            webSocketService.SendAsync(request);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending subscription request: {Message} {StackTrace}", ex.Message, ex.StackTrace);
        }
    }
}

