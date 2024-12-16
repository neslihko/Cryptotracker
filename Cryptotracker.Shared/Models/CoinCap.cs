namespace Cryptotracker.Shared.Models;


public class CoinCapResponse
{
    public List<CoinCapData> Data { get; set; } = new();
}

public class CoinCapData
{
    public int Rank { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Supply { get; set; } = "0";
    public string MaxSupply { get; set; } = "0";
    public string MarketCapUsd { get; set; } = "0";
    public string VolumeUsd24Hr { get; set; } = "0";
    public string PriceUsd { get; set; } = "0";
    public string ChangePercent24Hr { get; set; } = "0";
    public string Vwap24Hr { get; set; } = "0";
}