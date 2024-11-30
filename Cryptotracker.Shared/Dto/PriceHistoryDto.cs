namespace Cryptotracker.Shared.Dto;

public record PriceHistoryDto(
    decimal Price,
    DateTime Timestamp
);
