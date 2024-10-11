using System.Text.Json;
using Microsoft.Extensions.Options;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;
using Stocks.HTTP.Configuration;
using Stocks.HTTP.Http;
using Stocks.HTTP.Responses;

namespace Stocks.HTTP.Providers;

public sealed class MarketAssetsDataProvider(IHttpClientFactory httpClientFactory, IOptions<RequestConfiguration> options) : IMarketAssetsDataProvider
{
    public async Task<IEnumerable<MarketAsset>?> GetAllMarketAssetsAsync()
    {
        var httpClient = httpClientFactory.CreateClient("Instruments");
        var response = await httpClient.SendAsync(options.GetMarketAssetsRequest(HttpMethod.Get));

        if (!response.IsSuccessStatusCode || response.Content == null || response.Content.Headers.ContentLength == 0)
        {
            return null; // Return null if the response is not successful or if the content is empty
        }

        var content = await response.Content.ReadAsStringAsync();

        // If content is not valid JSON, return null instead of attempting to deserialize it
        if (string.IsNullOrWhiteSpace(content) || !IsValidJson(content))
        {
            return null;
        }
        var assets = JsonSerializer.Deserialize<MarketAssetsResponse>(content,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })?.Data;
        
        return assets;
    }
    
    private bool IsValidJson(string content)
    {
        try
        {
            JsonDocument.Parse(content);
            return true;
        }
        catch
        {
            return false;
        }
    }
}