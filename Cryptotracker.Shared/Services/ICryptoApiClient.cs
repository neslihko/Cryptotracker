using Cryptotracker.Shared.Dto;

namespace Cryptotracker.Shared.Services;

public interface ICryptoApiClient
{
    Task<IEnumerable<CryptoDto>> GetCurrentPrices();

}

