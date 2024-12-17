using Cryptotracker.Shared.Dto;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Cryptotracker.Api.Controllers
{
    /// <summary>
    /// Controller for managing cryptocurrency data
    /// </summary>
    /// [EnableCors("AllowReactApp")]
    [EnableCors]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class CryptosController : ControllerBase
    {
        private readonly IDatabaseService _dbService;
        private readonly ILogger<CryptosController> _logger;

        public CryptosController(IDatabaseService dbService, ILogger<CryptosController> logger)
        {
            _dbService = dbService;
            _logger = logger;
        }

        /// <summary>
        /// Get all cryptocurrencies with optional filtering and sorting
        /// </summary>
        /// <param name="search">Search by name or symbol</param>
        /// <param name="sortBy">Sort by: price, volume, marketCap</param>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CryptoDto>>> Get(
            [FromQuery] string? search,
            [FromQuery] string? sortBy)
        {
            try
            {
                // Validate sortBy parameter
                if (!string.IsNullOrEmpty(sortBy) &&
       !new[] { "price", "volume", "marketCap" }.Contains(sortBy.ToLower()))
                {
                    return BadRequest("Invalid sortBy parameter. Valid values are: price, volume, marketCap");
                }

                var result = await _dbService.GetCryptos(search?.Trim(), sortBy?.ToLower());

                if (Response != null)
                {
                    Response.Headers.Add("X-Total-Records", result.Count().ToString());
                }
                else
                {
                    // Handle the null case appropriately
                    _logger.LogWarning("Response object is null. Headers cannot be added.");
                }


                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cryptocurrencies");
                return StatusCode(500, new { error = "An error occurred while retrieving cryptocurrencies" });
            }
        }

        /// <summary>
        /// Get cryptocurrency details by symbol
        /// </summary>
        /// <param name="symbol">Cryptocurrency symbol (e.g., BTC)</param>
        [HttpGet("{symbol}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CryptoDto>> GetBySymbol(
            [Required][RegularExpression(@"^[A-Za-z0-9]+$")] string symbol)
        {
            try
            {
                var result = await _dbService.GetBySymbol(symbol.ToUpper());
                if (result == null)
                    return NotFound(new { error = $"Cryptocurrency with symbol {symbol} not found" });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cryptocurrency for symbol: {Symbol}", symbol);
                return StatusCode(500, new { error = "An error occurred while retrieving the cryptocurrency" });
            }
        }

        /// <summary>
        /// Get price history for a cryptocurrency
        /// </summary>
        /// <param name="symbol">Cryptocurrency symbol</param>
        /// <param name="from">Start date (optional)</param>
        /// <param name="to">End date (optional)</param>
        [HttpGet("{symbol}/history")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<PriceHistoryDto>>> GetHistory(
            [Required][RegularExpression(@"^[A-Za-z0-9]+$")] string symbol,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to)
        {
            try
            {
                // Validate date range
                if (from.HasValue && to.HasValue && from.Value > to.Value)
                {
                    return BadRequest(new { error = "From date must be before or equal to To date" });
                }

                var result = await _dbService.GetPriceHistory(symbol.ToUpper(), from, to);
                if (!result.Any())
                    return NotFound(new { error = $"No price history found for {symbol}" });


                if (Response != null)
                {
                    Response.Headers.Add("X-Date-Range-Start", from?.ToString("O") ?? "All");
                    Response.Headers.Add("X-Date-Range-End", to?.ToString("O") ?? "All");
                    Response.Headers.Add("X-Total-Records", result.Count().ToString());
                }
                else
                {
                    // Handle the null case appropriately
                    _logger.LogWarning("Response object is null. Headers cannot be added.");
                }
                // Add metadata headers


                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving price history for symbol: {Symbol}", symbol);
                return StatusCode(500, new { error = "An error occurred while retrieving price history" });
            }
        }
    }
}