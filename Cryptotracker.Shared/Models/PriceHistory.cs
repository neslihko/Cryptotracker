using System.ComponentModel.DataAnnotations.Schema;

namespace Cryptotracker.Shared.Models;

public class PriceHistory
{
    public int Id { get; set; }
    public int CryptocurrencyId { get; set; }

    [Column(TypeName = "decimal(40,8)")]  // Increased precision significantly
    public decimal PriceUsd { get; set; }

    public DateTime Timestamp { get; set; }
    public virtual Cryptocurrency? Cryptocurrency { get; set; }
}
