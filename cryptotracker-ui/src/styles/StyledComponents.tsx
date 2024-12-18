import styled from 'styled-components';

// TypeScript interfaces
interface SidePanelProps {
  $isDetailsVisible: boolean;
}

interface PriceChangeProps {
  $isPositive: boolean;
}

// Layout components
export const Layout = styled.div`
  display: grid;
  grid-template-columns: 350px 1fr;
  min-height: 100vh;
  overflow-y: hidden;
  
  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    height: 100vh;
  }
`;

export const MobileContainer = styled.div`
  @media (max-width: 768px) {
    position: relative;
    height: 100vh;
    overflow: hidden;
  }
`;

// Panel components
export const SidePanel = styled.div<SidePanelProps>`
  background-color: #f5f8fa;
  height: 100vh;
  overflow-y: auto;
  padding: 16px;
  box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);

  @media (max-width: 768px) {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100vh;
    z-index: 2;
    transform: translateX(${props => props.$isDetailsVisible ? '-100%' : '0'});
    transition: transform 0.3s ease-in-out;
  }
`;

export const MainPanel = styled.div<SidePanelProps>`
  background-color: #fff;
  overflow-y: auto;
  padding: 16px;
  height: calc(100vh - 32px);
  -ms-overflow-style: none;
  
  &::-webkit-scrollbar {
    display: none;
  }

  @media (max-width: 768px) {
    position: fixed;
    top: 0;
    left: 0;
    width: 100%;
    height: 100vh;
    z-index: 3;
    transform: translateX(${props => props.$isDetailsVisible ? '0' : '100%'});
    transition: transform 0.3s ease-in-out;
  }
`;

// Header components
export const TopBar = styled.div`
  background-color: #003d82;
  color: white;
  padding: 12px 20px;
  font-size: 24px;
  display: flex;
  align-items: center;
  justify-content: space-between;
  position: sticky;
  top: 0;
  z-index: 10;

  @media (max-width: 768px) {
    padding: 10px;
    font-size: 20px;
  }
`;

export const BackButton = styled.button`
  background: none;
  border: none;
  color: white;
  padding: 8px;
  cursor: pointer;
  position: absolute;
  left: 16px;
  font-size: 20px;
  
  @media (min-width: 769px) {
    display: none;
  }
`;

// Crypto item components
export const CryptoItem = styled.div`
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 16px;
  border-bottom: 1px solid #e1e4e8;
  cursor: pointer;
  transition: background-color 0.2s;

  &:hover {
    background-color: #f0f0f0;
  }

  @media (max-width: 768px) {
    padding: 12px;
  }
`;

export const CryptoLogo = styled.img`
  width: 24px;
  height: 24px;
  margin-right: 12px;
`;

export const CryptoInfo = styled.div`
  display: flex;
  flex-direction: column;
  flex: 1;
  min-width: 0;
`;

export const CryptoName = styled.div`
  font-weight: 500;
  margin-bottom: 4px;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
`;

export const CryptoLabel = styled.div`
  color: #666;
  font-size: 12px;
`;

// Price components
export const PriceInfo = styled.div`
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  min-width: 100px;
`;

export const Price = styled.div`
  font-size: 16px;
  font-weight: 500;
  margin-bottom: 4px;
`;

export const PriceChange = styled.span<PriceChangeProps>`
  color: ${props => props.$isPositive ? '#4CAF50' : '#F44336'};
  display: flex;
  align-items: center;
  gap: 4px;
  font-size: 14px;
  
  &::before {
    content: '';
    display: inline-block;
    width: 0;
    height: 0;
    border-left: 4px solid transparent;
    border-right: 4px solid transparent;
    border-bottom: ${props => props.$isPositive ? '6px solid #4CAF50' : 'none'};
    border-top: ${props => !props.$isPositive ? '6px solid #F44336' : 'none'};
  }
`;

// Input components
export const SearchInput = styled.input`
  width: calc(100% - 32px);
  margin: 16px;
  padding: 8px 12px;
  border: 1px solid #ccc;
  border-radius: 4px;
  font-size: 14px;
  
  &:focus {
    outline: none;
    border-color: #003366;
    box-shadow: 0 0 0 2px rgba(0, 51, 102, 0.1);
  }
`;

// Chart components
export const ChartContainer = styled.div`
  width: calc(100% - 32px);
  height: 300px;
  padding: 16px;
  margin: 16px;
  background: white;
  border: 1px solid #e1e4e8;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);

  @media (max-width: 768px) {
    width: calc(100% - 16px);
    height: 250px;
    margin: 8px;
    padding: 12px;
  }
`;

// Utility components
export const LoadingSpinner = styled.div`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100px;
  color: #666;
`;

export const ErrorMessage = styled.div`
  color: #F44336;
  text-align: center;
  padding: 20px;
  background-color: #ffebee;
  border-radius: 4px;
  margin: 16px;
`;
// Add these to your existing styled-components file

export const DetailContainer = styled.div`
  padding: 20px;
  max-width: 1200px;
  margin: 0 auto;

  @media (max-width: 768px) {
    padding: 16px;
    margin-top: 60px; // Account for the TopBar
  }
`;

export const DetailHeader = styled.div`
  display: flex;
  align-items: center;
  margin-bottom: 24px;
  gap: 16px;
`;

export const DetailInfo = styled.div`
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
  gap: 16px;
  margin-bottom: 24px;
  
  @media (max-width: 768px) {
    grid-template-columns: 1fr 1fr;
  }
`;

export const InfoCard = styled.div`
  background: white;
  padding: 16px;
  border-radius: 8px;
  border: 1px solid #e1e4e8;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
`;

export const InfoLabel = styled.div`
  color: #666;
  font-size: 12px;
  margin-bottom: 4px;
`;

export const InfoValue = styled.div`
  font-size: 16px;
  font-weight: 500;
`;

export const DetailDescription = styled.div`
  background: white;
  padding: 20px;
  border-radius: 8px;
  border: 1px solid #e1e4e8;
  margin-bottom: 24px;
  line-height: 1.6;

  @media (max-width: 768px) {
    padding: 16px;
  }
`;

export const DetailChart = styled(ChartContainer)`
  margin: 24px 0;
`;
const styledComponents =  {
  DetailContainer,
  DetailHeader,
  DetailInfo,
  InfoCard,
  InfoLabel,
  InfoValue,
  DetailDescription,
  DetailChart,
  Layout,
  MobileContainer,
  SidePanel,
  MainPanel,
  TopBar,
  BackButton,
  CryptoItem,
  CryptoLogo,
  CryptoInfo,
  CryptoName,
  CryptoLabel,
  PriceInfo,
  Price,
  PriceChange,
  SearchInput,
  ChartContainer,
  LoadingSpinner,
  ErrorMessage
};

export default styledComponents

