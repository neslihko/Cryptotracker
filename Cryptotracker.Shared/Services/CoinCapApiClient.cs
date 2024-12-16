using Cryptotracker.Shared.Configuration;
using Cryptotracker.Shared.Dto;
using Cryptotracker.Shared.Models;
using Cryptotracker.Shared.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Json;

public class CoinCapApiClient : ICryptoApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CoinCapApiClient> _logger;
    private readonly CoinCapSettings _settings;

    public CoinCapApiClient(
        HttpClient httpClient,
        ILogger<CoinCapApiClient> logger,
        IOptions<ApiSettings> settings)
    {
        _httpClient = httpClient;
        _logger = logger;
        _settings = settings.Value.CoinCap;
    }

    private decimal SafeParseDecimal(string value, decimal defaultValue = 0)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Equals("null", StringComparison.OrdinalIgnoreCase))
            return defaultValue;

        return decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result)
            ? result
            : defaultValue;
    }
    public async Task<IEnumerable<CryptoDto>> GetCurrentPrices()
    {
        try
        {
            _logger.LogInformation("Fetching prices from CoinCap API");

            var response = await _httpClient.GetAsync(_settings.AssetsEndpoint);
            _ = response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<CoinCapResponse>();

            if (content?.Data == null || !content.Data.Any())
            {
                _logger.LogWarning("No data received from CoinCap API");
                return Enumerable.Empty<CryptoDto>();
            }
            var results = content.Data.Select(d => new CryptoDto(
                symbol: d.Symbol,
                name: d.Name,
                rank: int.TryParse(d.Rank.ToString(), out var rank) ? rank : 0,
                currentPrice: SafeParseDecimal(d.PriceUsd),
                volumeUsd24Hr: SafeParseDecimal(d.VolumeUsd24Hr),
                marketCap: SafeParseDecimal(d.MarketCapUsd),
                changePercent24Hr: SafeParseDecimal(d.ChangePercent24Hr),
                supply: SafeParseDecimal(d.Supply),
                maxSupply: SafeParseDecimal(d.MaxSupply),
                vwap24Hr: SafeParseDecimal(d.Vwap24Hr),
                lastUpdated: DateTime.UtcNow,
                null
            ))
            .Where(dto =>
                dto.CurrentPrice >= 0 &&
                dto.VolumeUsd24Hr >= 0 &&
                dto.MarketCap >= 0 &&
                dto.Supply >= 0 &&
                (dto.MaxSupply == 0 || dto.Supply <= dto.MaxSupply) // Validates supply
            ).ToList();


            _logger.LogInformation("Successfully processed {Count} cryptocurrencies",
                results.Count);

            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching data from CoinCap API");
            throw;
        }
    }






}