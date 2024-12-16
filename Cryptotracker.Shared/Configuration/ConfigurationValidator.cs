using Cryptotracker.Shared.Configuration;

public static class ConfigurationValidator
{
    public static void ValidateApiSettings(ApiSettings settings)
    {
        if (string.IsNullOrEmpty(settings.CoinCap.BaseUrl))
            throw new InvalidOperationException("CoinCap BaseUrl is not configured");

        if (string.IsNullOrEmpty(settings.CoinCap.AssetsEndpoint))
            throw new InvalidOperationException("CoinCap AssetsEndpoint is not configured");

        if (settings.CoinCap.UpdateIntervalMinutes <= 0)
            throw new InvalidOperationException("UpdateIntervalMinutes must be greater than 0");
    }
}