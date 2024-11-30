
using Cryptotracker.Shared.Models;
using Microsoft.EntityFrameworkCore;
namespace Cryptotracker.Shared.Data;

public class CryptoDbContext : DbContext
{
    public CryptoDbContext(DbContextOptions<CryptoDbContext> options)
        : base(options) { }

    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }
    public DbSet<PriceHistory> PriceHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<Cryptocurrency>(entity
 =>
        {
            _ = entity.HasIndex(e => e.Symbol).IsUnique();
            _ = entity.Property(e => e.Name);
            _ = entity.Property(e => e.Rank);
            _ = entity.Property(e => e.CurrentPriceUsd).HasPrecision(18, 8);
            _ = entity.Property(e => e.VolumeUsd).HasPrecision(18, 2);
            _ = entity.Property(e => e.MarketCapUsd).HasPrecision(18, 2);
            _ = entity.Property(e => e.ChangePercent24Hr).HasPrecision(18, 8);
            _ = entity.Property(e => e.Supply).HasPrecision(18, 8);
            _ = entity.Property(e => e.MaxSupply).HasPrecision(18, 8);
            _ = entity.Property(e => e.VWAP24Hr).HasPrecision(18, 8);
            _ = entity.HasMany(e => e.PriceHistory)
                .WithOne(ph => ph.Cryptocurrency)
                .HasForeignKey(ph => ph.CryptocurrencyId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        _ = modelBuilder.Entity<PriceHistory>()
        .HasOne(p => p.Cryptocurrency)
        .WithMany(c => c.PriceHistory) // Eine Kryptowährung hat viele Preishistorien
        .HasForeignKey(p => p.CryptocurrencyId) // Fremdschlüssel
        .OnDelete(DeleteBehavior.Cascade);
    }
}