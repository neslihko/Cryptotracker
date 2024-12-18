// src/components/CryptoList.tsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import { getCryptos } from '../services/api';
import { Crypto } from '../types/crypto';
import {
  Layout,
  SidePanel,
  MainPanel,
  SearchInput,
  CryptoItem,
  CryptoInfo,
  CryptoLogo,  // Updated name
  PriceChange,
  TopBar,
} from '../styles/StyledComponents';

interface CryptoListProps {
  isDetailsVisible?: boolean; // Optional now
}
const CryptoList: React.FC<CryptoListProps> = ({ isDetailsVisible = false }) => {  // Add default value
  const navigate = useNavigate();
  const [cryptos, setCryptos] = React.useState<Crypto[]>([]);
  const [search, setSearch] = React.useState('');
  const [loading, setLoading] = React.useState(true);

  React.useEffect(() => {
    const fetchData = async () => {
      try {
        setLoading(true);
        const data = await getCryptos(search);
        setCryptos(Array.isArray(data) ? data : []);
      } catch (error) {
        console.error('Error:', error);
        setCryptos([]);
      } finally {
        setLoading(false);
      }
    };

    const timer = setTimeout(fetchData, 300);
    return () => clearTimeout(timer);
  }, [search]);

  return (
    <Layout>
      <SidePanel $isDetailsVisible={isDetailsVisible}>

        <TopBar>Übersicht</TopBar>
        <SearchInput
          type="text"
          placeholder="Suche..."
          value={search}
          onChange={(e) => setSearch(e.target.value)}
        />
        {loading ? (
          <div>Loading...</div>
        ) : (
          cryptos.map(crypto => (
            <CryptoItem
              key={crypto.symbol}
              onClick={() => navigate(`/crypto/${crypto.symbol}`)}
            >
              <CryptoInfo>
                
                <div>
                  <div> <CryptoLogo src={crypto?.logoUrl} alt={crypto?.name} />{crypto.name}</div>
                  <div style={{ color: '#666', fontSize: '0.9em' }}>
                    Aktueller Kurs:
                  </div>
                </div>
              </CryptoInfo>
              <div>
                <PriceChange $isPositive={crypto.changePercent24Hr  > 0}>

                  {crypto.changePercent24Hr > 0 ? '+' : ''}
                  <div style={{ color: '#666', fontSize: '0.9em' }}>
                  ${crypto.currentPrice.toLocaleString()}
                  </div>
                </PriceChange> 
              </div>
            </CryptoItem>
          ))
        )}
      </SidePanel>
      <MainPanel $isDetailsVisible={isDetailsVisible}>
        {/* Details view will be rendered here by nested routes */}
      </MainPanel>
    </Layout>
  );
};

export default CryptoList;