// src/services/api.test.ts
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { api, getCryptoHistory } from './services/api';
import { console } from 'inspector';

const mock = new MockAdapter(api);

describe('API Services', () => {
  const consoleSpy = jest.spyOn(console, 'error').mockImplementation();

  beforeEach(() => {
    mock.reset();
    consoleSpy.mockClear();
  });

  afterAll(() => {
    consoleSpy.mockRestore();
    mock.restore();
  });

  describe('getCryptoHistory', () => {
    const mockData = [
      { price: 50000,timestamp: '2024-11-01' },
      {  price: 51000,timestamp: '2024-11-02' }
    ];

    const baseUrl = '/Cryptos/BTC/history';

    it('should fetch price history successfully', async () => {
      mock.onGet(baseUrl).reply(200, mockData);

      const result = await getCryptoHistory('BTC');
      
      expect(result).toEqual(mockData);
    });

    it('should handle API errors (500)', async () => {
      mock.onGet(baseUrl).reply(500);

      await expect(getCryptoHistory('BTC')).rejects.toThrow(
        'Failed to fetch history for BTC: Request failed with status code 500'
      );
    });

    it('should handle network errors', async () => {
      mock.onGet(baseUrl).networkError();

      await expect(getCryptoHistory('BTC')).rejects.toThrow(
        'No response received from API'
      );
    });

    it('should handle timeout errors', async () => {
      mock.onGet(baseUrl).timeout();

      await expect(getCryptoHistory('BTC')).rejects.toThrow(
        'No response received from API'
      );
    });

    it('Request failed with status code 404', async () => {
      mock.onGet(baseUrl).reply(404);

      await expect(getCryptoHistory('BTC')).rejects.toThrowError(
        'API request failed with status 404'
      );
    });
 

  
  });
});