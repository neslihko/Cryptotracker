using Cryptotracker.Shared.Data;
using Cryptotracker.Shared.Dto;
using Cryptotracker.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public interface IDatabaseService
{
    Task<IEnumerable<CryptoDto>> GetCryptos(string? search = null, string? sortBy = null);
    Task<CryptoDto?> GetBySymbol(string symbol);
    Task<IEnumerable<PriceHistoryDto>> GetPriceHistory(
        string symbol,
        DateTime? from = null,
        DateTime? to = null);
    Task UpdatePrices(IEnumerable<CryptoDto> prices);
    Task AddHistoricalData(string symbol, IEnumerable<PriceHistoryDto> history);

}

public class DatabaseService : IDatabaseService
{
    private readonly CryptoDbContext _context;
    private readonly ILogger<DatabaseService> _logger;

    public DatabaseService(CryptoDbContext context, ILogger<DatabaseService> logger)
    {
        _context = context;
        _logger = logger;
    }

    private static string GenerateCryptoLogoUrl(string name, string symbol)
    {
        return $"https://cryptologos.cc/logos/{name.ToLower()}-{symbol.ToLower()}-logo.png";
    }

    public async Task<IEnumerable<CryptoDto>> GetCryptos(string? search = null, string? sortBy = null)
    {
        try
        {
            var query = _context.Cryptocurrencies.AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToUpper();
                query = query.Where(c =>
                    c.Name.ToUpper().Contains(search) ||
                    c.Symbol.ToUpper().Contains(search));
            }

            // Apply sorting
            query = sortBy?.ToLower() switch
            {
                "price" => query.OrderByDescending(c => c.CurrentPriceUsd),
                "volume" => query.OrderByDescending(c => c.VolumeUsd),
                "marketcap" => query.OrderByDescending(c => c.MarketCapUsd),
                "name" => query.OrderBy(c => c.Name),
                "symbol" => query.OrderBy(c => c.Symbol),
                _ => query.OrderByDescending(c => c.MarketCapUsd)
            };

            return await query
                .Select(c => new CryptoDto(
                c.Symbol,
                c.Name,
                c.Rank,
                c.CurrentPriceUsd,
                c.VolumeUsd,
                c.MarketCapUsd,
                c.ChangePercent24Hr,
                c.Supply,

                 c.MaxSupply,
                 c.VWAP24Hr, c.LastUpdated, GenerateCryptoLogoUrl(c.Name, c.Symbol)))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cryptocurrencies");
            throw;
        }
    }

    public async Task UpdatePrices(IEnumerable<CryptoDto> prices)
    {
        try
        {
            foreach (var price in prices)
            {
                // Find existing crypto or create new one
                var crypto = await _context.Cryptocurrencies
                    .FirstOrDefaultAsync(c => c.Symbol == price.Symbol);

                if (crypto == null)
                {
                    crypto = new Cryptocurrency
                    {
                        Symbol = price.Symbol,
                        Name = price.Name,
                        CurrentPriceUsd = price.CurrentPrice,
                        VolumeUsd = price.VolumeUsd24Hr,
                        MarketCapUsd = price.MarketCap,
                        LastUpdated = DateTime.UtcNow
                    };
                    _ = _context.Cryptocurrencies.Add(crypto);
                    // Save immediately to get the Id
                    _ = await _context.SaveChangesAsync();
                }
                else
                {
                    crypto.CurrentPriceUsd = price.CurrentPrice;
                    crypto.VolumeUsd = price.VolumeUsd24Hr;
                    crypto.MarketCapUsd = price.MarketCap;
                    crypto.LastUpdated = DateTime.UtcNow;
                    _ = await _context.SaveChangesAsync();
                }

                // Now add price history after crypto is saved
                var history = new PriceHistory
                {
                    CryptocurrencyId = crypto.Id, // Now we have a valid Id
                    PriceUsd = price.CurrentPrice,
                    Timestamp = DateTime.UtcNow
                };
                _ = _context.PriceHistory.Add(history);
            }

            // Save any remaining changes
            _ = await _context.SaveChangesAsync();
            _logger.LogInformation($"Updated prices for {prices.Count()} cryptocurrencies");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating cryptocurrency prices");
            throw;
        }
    }
    public async Task<CryptoDto?> GetBySymbol(string symbol)
    {
        try
        {
            return await _context.Cryptocurrencies
                .Where(c => c.Symbol == symbol.ToUpper())
                .Select(c => new CryptoDto(
                c.Symbol,
                c.Name,
                 c.Rank,
                c.CurrentPriceUsd,
                c.VolumeUsd,
                c.MarketCapUsd,
                c.ChangePercent24Hr,
                c.Supply,
                 c.MaxSupply,
                 c.VWAP24Hr, c.LastUpdated, GenerateCryptoLogoUrl(c.Name, c.Symbol)))
                .FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cryptocurrency by symbol: {Symbol}", symbol);
            throw;
        }
    }

    public async Task<IEnumerable<PriceHistoryDto>> GetPriceHistory(string symbol)
    {
        try
        {
            return await _context.PriceHistory
                .Include(p => p.Cryptocurrency)
                .Where(p => p.Cryptocurrency != null &&
           !string.IsNullOrEmpty(p.Cryptocurrency.Symbol) &&
           !string.IsNullOrEmpty(symbol) &&
           p.Cryptocurrency.Symbol == symbol.ToUpper())
                .OrderByDescending(p => p.Timestamp)
                .Select(p => new PriceHistoryDto(
                    p.PriceUsd,
                    p.Timestamp))
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting price history for symbol: {Symbol}", symbol);
            throw;
        }
    }


    public async Task AddHistoricalData(string symbol, IEnumerable<PriceHistoryDto> history)
    {
        try
        {
            var crypto = await _context.Cryptocurrencies
                .FirstOrDefaultAsync(c => c.Symbol == symbol.ToUpper());

            if (crypto == null)
                throw new KeyNotFoundException($"Cryptocurrency with symbol {symbol} not found");

            var historicalData = history.Select(h => new PriceHistory
            {
                Id = crypto.Id,
                PriceUsd = h.Price,
                Timestamp = h.Timestamp
            });

            await _context.PriceHistory.AddRangeAsync(historicalData);
            _ = await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding historical data for symbol: {Symbol}", symbol);
            throw;
        }
    }

    public async Task<IEnumerable<PriceHistoryDto>> GetPriceHistory(
    string symbol,
    DateTime? from = null,
    DateTime? to = null)
    {
        try
        {
            var query = _context.PriceHistory
                .Include(p => p.Cryptocurrency)
                .Where(p => p.Cryptocurrency != null &&
           !string.IsNullOrEmpty(p.Cryptocurrency.Symbol) &&
           !string.IsNullOrEmpty(symbol) &&
           p.Cryptocurrency.Symbol == symbol.ToUpper());

            // Apply date filters if provided
            if (from.HasValue)
            {
                query = query.Where(p => p.Timestamp >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(p => p.Timestamp <= to.Value);
            }

            // Order by timestamp descending and map to DTO
            var history = await query
                .OrderByDescending(p => p.Timestamp)
                .Select(p => new PriceHistoryDto(
                    p.PriceUsd,  // Remove named argument
                    p.Timestamp  // Remove named argument
                ))
                .ToListAsync();

            if (!history.Any())
            {
                _logger.LogWarning("No price history found for symbol: {Symbol}", symbol);
                return Enumerable.Empty<PriceHistoryDto>();
            }

            _logger.LogInformation(
                "Retrieved {Count} price history records for {Symbol}",
                history.Count,
                symbol);

            return history;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving price history for symbol: {Symbol}", symbol);
            throw;
        }
    }
}