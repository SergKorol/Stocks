namespace Stocks.WebSockets.Models;

public record WebSocketOptions
{
    public required string BaseUrl { get; set; }
    public string? Token { get; set; }
}