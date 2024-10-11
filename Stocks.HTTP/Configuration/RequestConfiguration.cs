namespace Stocks.HTTP.Configuration;

public record RequestConfiguration
{
    public string? BaseUrl { get; set; }
    public string? Token { get; set; }
}