using Cryptotracker.Api.Controllers;
using Cryptotracker.Shared.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace Cryptotracker.Api.Tests.Controllers
{
    [TestFixture]
    public class CryptosControllerTests
    {
        private Mock<IDatabaseService> _mockDbService;
        private Mock<ILogger<CryptosController>> _mockLogger;
        private CryptosController _controller;
        private DateTime _testDateTime;

        [SetUp]
        public void Setup()
        {
            _mockDbService = new Mock<IDatabaseService>();
            _mockLogger = new Mock<ILogger<CryptosController>>();
            _controller = new CryptosController(_mockDbService.Object, _mockLogger.Object);
            _testDateTime = DateTime.UtcNow;

            // Initialize the controller context
            var context = new DefaultHttpContext();
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        [Test]
        public async Task Get_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var expectedCryptos = new List<CryptoDto>
            {
                new CryptoDto(
                    symbol: "BTC",
                    name: "Bitcoin",
                    rank: 1,
                    currentPrice: 50000.00m,
                    volumeUsd24Hr: 30000000000m,
                    marketCap: 1000000000000m,
                    changePercent24Hr: 2.5m,
                    supply: 19000000m,
                    maxSupply: 21000000m,
                    vwap24Hr: 49500.00m,
                    lastUpdated: _testDateTime,
                    logoUrl: "https://example.com/btc.png"
                )
            };

            _ = _mockDbService.Setup(x => x.GetCryptos(It.IsAny<string>(), It.IsAny<string>()))
                         .ReturnsAsync(expectedCryptos);

            // Act
            var result = await _controller.Get(search: "BTC", sortBy: "price");

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            var returnedCryptos = okResult.Value as IEnumerable<CryptoDto>;
            Assert.That(returnedCryptos.First().Symbol, Is.EqualTo("BTC"));
        }


        [Test]
        public async Task Get_WithValidSortByPrice_ReturnsOkResult()
        {
            // Arrange
            var expectedCryptos = new List<CryptoDto>
            {
                new CryptoDto("BTC", "Bitcoin", 1, 50000.00m, 30000000000m, 1000000000000m, 2.5m, 19000000m, 21000000m, 49500.00m, _testDateTime, null)
            };

            _ = _mockDbService.Setup(x => x.GetCryptos(It.IsAny<string>(), "price"))
                         .ReturnsAsync(expectedCryptos);

            // Act
            var result = await _controller.Get(null, "price");

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(okResult.Value, Is.Not.Null);
        }
        [Test]
        public async Task Get_WithInvalidSortBy_ReturnsBadRequest()
        {
            // Arrange
            var sortBy = "invalidSort";

            // Act
            var result = await _controller.Get(null, sortBy);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task GetBySymbol_WithValidSymbol_ReturnsOkResult()
        {
            // Arrange
            var expectedCrypto = new CryptoDto(
                symbol: "BTC",
                name: "Bitcoin",
                rank: 1,
                currentPrice: 50000.00m,
                volumeUsd24Hr: 30000000000m,
                marketCap: 1000000000000m,
                changePercent24Hr: 2.5m,
                supply: 19000000m,
                maxSupply: 21000000m,
                vwap24Hr: 49500.00m,
                lastUpdated: _testDateTime,
                logoUrl: "https://example.com/btc.png"
            );

            _ = _mockDbService.Setup(x => x.GetBySymbol("BTC"))
                         .ReturnsAsync(expectedCrypto);

            // Act
            var result = await _controller.GetBySymbol("BTC");

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            var returnedCrypto = okResult.Value as CryptoDto;
            Assert.That(returnedCrypto.Symbol, Is.EqualTo("BTC"));
        }

        [Test]
        public async Task GetHistory_WithValidParameters_ReturnsOkResult()
        {
            // Arrange
            var expectedHistory = new List<PriceHistoryDto>
            {
                new PriceHistoryDto(50000.00m, _testDateTime.AddHours(-2)),
                new PriceHistoryDto(51000.00m, _testDateTime.AddHours(-1)),
                new PriceHistoryDto(49500.00m, _testDateTime)
            };

            var fromDate = _testDateTime.AddDays(-7);
            var toDate = _testDateTime;

            _ = _mockDbService.Setup(x => x.GetPriceHistory("BTC", fromDate, toDate))
                         .ReturnsAsync(expectedHistory);

            // Act
            var result = await _controller.GetHistory("BTC", fromDate, toDate);

            // Assert
            Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            var returnedHistory = okResult.Value as IEnumerable<PriceHistoryDto>;
            Assert.That(returnedHistory.Count(), Is.EqualTo(expectedHistory.Count));
        }

        [Test]
        public async Task Get_WhenExceptionOccurs_ReturnsInternalServerError()
        {
            // Arrange
            _ = _mockDbService.Setup(x => x.GetCryptos(It.IsAny<string>(), It.IsAny<string>()))
                         .ThrowsAsync(new System.Exception("Database error"));

            // Act
            var result = await _controller.Get(null, null);

            // Assert
            Assert.That(result.Result, Is.TypeOf<ObjectResult>());
            var statusResult = result.Result as ObjectResult;
            Assert.That(statusResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
        }

        [Test]
        public async Task GetHistory_WithInvalidDateRange_ReturnsBadRequest()
        {
            // Arrange
            var fromDate = _testDateTime;
            var toDate = _testDateTime.AddDays(-7);

            // Act
            var result = await _controller.GetHistory("BTC", fromDate, toDate);

            // Assert
            Assert.That(result.Result, Is.TypeOf<BadRequestObjectResult>());
            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        }

        [Test]
        public async Task GetHistory_WithNoData_ReturnsNotFound()
        {
            // Arrange
            _ = _mockDbService.Setup(x => x.GetPriceHistory(
                It.IsAny<string>(),
                It.IsAny<DateTime?>(),
                It.IsAny<DateTime?>()))
                .ReturnsAsync(new List<PriceHistoryDto>());

            // Act
            var result = await _controller.GetHistory("BTC", null, null);

            // Assert
            Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
            var notFoundResult = result.Result as NotFoundObjectResult;
            Assert.That(notFoundResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        }
    }
}