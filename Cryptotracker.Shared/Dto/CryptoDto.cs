namespace Cryptotracker.Shared.Dto;

public class CryptoDto
{
    public string Symbol { get; set; }
    public string Name { get; set; }
    public int Rank { get; set; }
    public decimal CurrentPrice { get; set; }
    public decimal VolumeUsd24Hr { get; set; }
    public decimal MarketCap { get; set; }
    public decimal ChangePercent24Hr { get; set; }
    public decimal Supply { get; set; }
    public decimal MaxSupply { get; set; }
    public decimal VWAP24Hr { get; set; }
    public DateTime LastUpdated { get; set; }
    public string LogoUrl { get; set; }


    public CryptoDto(string symbol, string name, int rank, decimal currentPrice, decimal volumeUsd24Hr,
                     decimal marketCap, decimal changePercent24Hr, decimal supply, decimal maxSupply, decimal vwap24Hr,
                     DateTime lastUpdated, string? logoUrl)
    {
        Symbol = symbol;
        Name = name;
        Rank = rank;
        CurrentPrice = currentPrice;
        VolumeUsd24Hr = volumeUsd24Hr;
        MarketCap = marketCap;
        ChangePercent24Hr = changePercent24Hr;
        Supply = supply;
        MaxSupply = maxSupply;
        VWAP24Hr = vwap24Hr;
        LastUpdated = lastUpdated;
        LogoUrl = logoUrl ?? "default-logo.png";
    }
}
