
import axios, { AxiosError } from 'axios';

export const api = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL || 'https://cryptotracker-dx91.onrender.com/api/',
  timeout: 30000
});

const logError = (error: unknown) => {
  if (process.env.NODE_ENV !== 'test') {
    console.error('API Error:', error);
  }
};

api.interceptors.response.use(
  response => response,
  (error: AxiosError) => {
    if (error.response) {
      logError({
        message: error.message,
        status: error.response?.status,
        data: error.response?.data
      });
    }
    return Promise.reject(error);
  }
);
 
 
export const getCryptos = async (search?: string, sortBy?: string) => {
    try {
        const params = new URLSearchParams();
        if (search) params.append('search', search);
        if (sortBy) params.append('sortBy', sortBy);
        
        console.log('Fetching cryptos with params:', params.toString());
        const response = await api.get(`/cryptos?${params}`);
        return response.data;
    } catch (error) {
        console.error('Error in getCryptos:', error);
        throw error;
    }
};

export const getCryptoDetails = async (symbol: string) => {
    const response = await api.get(`/cryptos/${symbol}`);
    return response.data;
};

export const getCryptoHistory = async (symbol: string, range: string = '7d') => {
  try {
    const response = await api.get(`/Cryptos/${symbol}/history`, {
      params: { range }
    });
    
    if (response.status === 204) {
      return [];
    }
    
    return response.data;
  } catch (error) {
    if (error instanceof AxiosError) {
      if (!error.response || error.code === 'ECONNABORTED') {
        throw  Error('No response received from API');
      }
      if (error.response.status === 404) {
        const errorMessage = `API request failed with status ${error.response.status}`;
        throw new Error(errorMessage);

      }
      if (error.response.status === 500) {
        throw new Error(`Failed to fetch history for ${symbol}: Request failed with status code 500`);
      }
      throw new Error(`Failed to fetch history for ${symbol}: ${error.message}`);
    }
    throw error;
  }
};