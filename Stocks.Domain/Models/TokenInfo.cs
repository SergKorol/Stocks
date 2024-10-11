using System.Text.Json.Serialization;

namespace Stocks.Domain.Models;

public record TokenInfo
{
    [JsonPropertyName("access_token")]
    public string? AccessToken { get; set; }
}