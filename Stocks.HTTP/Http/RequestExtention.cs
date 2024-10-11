using System.Net.Http.Headers;
using Microsoft.Extensions.Options;
using Stocks.HTTP.Configuration;

namespace Stocks.HTTP.Http;

public static class RequestExtention
{
    public static HttpRequestMessage GetMarketAssetsRequest(this IOptions<RequestConfiguration> options, HttpMethod httpMethod)
    {
        if (options.Value.BaseUrl != null)
        {
            var message = new HttpRequestMessage
            {
                Method = httpMethod,
                RequestUri = new Uri(options.Value.BaseUrl)
            };
            message.Headers.Add("Accept", "application/json");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.Value.Token);
        
            return message;
        }
        return new HttpRequestMessage();
    }
}