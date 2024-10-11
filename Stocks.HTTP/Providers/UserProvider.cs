using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stocks.Domain.Contracts;
using Stocks.Domain.Models;
using Stocks.HTTP.Configuration;

namespace Stocks.HTTP.Providers;

public class UserProvider(
    IHttpClientFactory httpClientFactory,
    IOptions<AuthConfiguration> authOptions,
    ILogger<UserProvider> logger) : IUserProvider
{
    public async Task SetUserToken(string? username, string? password)
    {
        var httpClient = httpClientFactory.CreateClient("User");
        var formData = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string?>("grant_type", "password"),
            new KeyValuePair<string, string?>("client_id", "app-cli"),
            new KeyValuePair<string, string?>("username", username),
            new KeyValuePair<string, string?>("password", password)
        });

        httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));

        try
        {
            var response = await httpClient.PostAsync(new Uri(authOptions.Value.AuthUrl), formData);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var token = JsonSerializer.Deserialize<TokenInfo>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            SaveTokenToAppSettings(token?.AccessToken);
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Request failed: {ExMessage}", ex.Message);
        }
    }

    private void SaveTokenToAppSettings(string? token)
    {
        try
        {
            var configJson = File.ReadAllText("../../Stocks/Stocks.API/appsettings.json");

            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configJson);
            if (config == null) return;
            if (token != null) config["AccessToken"] = token;

            var updatedConfigJson = JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("appsettings.json", updatedConfigJson);
        }
        catch (JsonException ex)
        {
            logger.LogError(ex, "Failed to update appsettings.json: {ExMessage}", ex.Message);
        }
    }
}