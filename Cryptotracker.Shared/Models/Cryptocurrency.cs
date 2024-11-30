using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptotracker.Shared.Models;

public class Cryptocurrency
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Rank { get; set; }

    [Column(TypeName = "decimal(40,8)")]
    public decimal CurrentPriceUsd { get; set; }

    [Column(TypeName = "decimal(40,2)")]
    public decimal VolumeUsd { get; set; }

    [Column(TypeName = "decimal(40,2)")]
    public decimal MarketCapUsd { get; set; }

    [Column(TypeName = "decimal(40,8)")]
    public decimal ChangePercent24Hr { get; set; }

    [Column(TypeName = "decimal(40,8)")]
    public decimal Supply { get; set; }

    [Column(TypeName = "decimal(40,8)")]
    public decimal MaxSupply { get; set; }

    [Column(TypeName = "decimal(40,8)")]
    public decimal VWAP24Hr { get; set; }

    public DateTime LastUpdated { get; set; }


    public virtual ICollection<PriceHistory> PriceHistory { get; set; } = new List<PriceHistory>();
}