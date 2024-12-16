import React, { useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getCryptoDetails, getCryptoHistory } from '../services/api';
import {
  TopBar,
  BackButton,
  DetailContainer,
  Price,
  PriceChange,
  CryptoItem,
  CryptoLogo,
  CryptoInfo,
  LoadingSpinner,
  ErrorMessage,
} from '../styles/StyledComponents';
import PriceChart from './PriceChart';
import { Crypto,PriceHistory } from '../types/crypto';
import { AdditionalInfo } from './AdditionalInfo';

const CryptoDetails: React.FC = () => {
  const { symbol } = useParams<{ symbol: string }>();
  const navigate = useNavigate();

  // State hooks
  const [crypto, setCrypto] = React.useState<Crypto | null>(null);
  const [history, setHistory] = React.useState<PriceHistory[]>([]);
  const [loading, setLoading] = React.useState(true);
  const [historyLoading, setHistoryLoading] = React.useState(false);
  const [error, setError] = React.useState<string | null>(null);
  const [selectedRange, setSelectedRange] = useState('7d');

  const ranges = [
    { label: '7 Days', value: '7d' },
    { label: '30 Days', value: '30d' },
    { label: 'All Time', value: '' },
  ];

  React.useEffect(() => {
    if (!symbol) {
      setError('Invalid cryptocurrency symbol');
      return;
    }

    const fetchCryptoDetails = async () => {
      try {
        setLoading(true);
        const data = await getCryptoDetails(symbol);
        setCrypto(data);
        setError(null);
      } catch (err) {
        setError('Failed to fetch cryptocurrency details');
      } finally {
        setLoading(false);
      }
    };

    fetchCryptoDetails();
  }, [symbol]);

  React.useEffect(() => {
    if (!symbol) {
      setError('Invalid cryptocurrency symbol');
      return;
    }

    const fetchCryptoHistory = async () => {
      try {
        setHistoryLoading(true);
        const data = await getCryptoHistory(symbol, selectedRange);
        setHistory(data || []);
        setError(null);
      } catch (err) {
        setError('Failed to fetch historical data');
      } finally {
        setHistoryLoading(false);
      }
    };

    fetchCryptoHistory();
  }, [symbol, selectedRange]);

  if (!symbol) return <ErrorMessage>Invalid or missing cryptocurrency symbol</ErrorMessage>;
  if (loading) return <LoadingSpinner>Loading details...</LoadingSpinner>;
  if (error) return <ErrorMessage>{error}</ErrorMessage>;

  return (
    <DetailContainer>
      <TopBar>
        <center>Detail</center>
        <BackButton onClick={() => navigate(-1)}>←</BackButton>
        </TopBar>
        <CryptoItem
              key={crypto?.symbol}
              onClick={() => navigate(`/crypto/${crypto?.symbol}`)}
            >
              <CryptoInfo>
                
                <div>
                  <div> <CryptoLogo src={crypto?.logoUrl} alt={crypto?.name} />{crypto?.name}</div>
                  <div style={{ color: '#666', fontSize: '0.9em' }}>
                    Aktueller Kurs:
                  </div>
                </div>
              </CryptoInfo>
              <div>
              <PriceChange isPositive={(crypto?.changePercent24Hr ?? 0) >= 0}>
          {(crypto?.changePercent24Hr ?? 0) > 0 ? '+' : ''}
          {(crypto?.changePercent24Hr ?? 0).toFixed(2)}%
        </PriceChange>
              </div>
            </CryptoItem>
       
            <div>
      
      <PriceChart data={history} />
      </div>
      {crypto && (
  <>
    {/* ... bestehender Code ... */}
    <AdditionalInfo
      marketCap={crypto.marketCap}
      volume24h={crypto.volumeUsd24Hr}
      supply={crypto.supply}
      maxSupply={crypto.maxSupply}
      vwap24h={crypto.vwaP24Hr}
      rank={crypto.rank}
    />
  </>
)}
    </DetailContainer>
  );
};

export default CryptoDetails;