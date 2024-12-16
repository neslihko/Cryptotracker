namespace Cryptotracker.Shared.Configuration;

public class ApiSettings
{
    public CoinCapSettings CoinCap { get; set; } = new();
    public UISettings UI { get; set; } = new();
}

public class CoinCapSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string AssetsEndpoint { get; set; } = string.Empty;
    public int UpdateIntervalMinutes { get; set; }
    public string AcceptHeader { get; set; } = string.Empty;
}

public class UISettings
{
    public string BaseUrl { get; set; } = string.Empty;

}