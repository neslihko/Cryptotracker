import styled from 'styled-components';
interface SidePanelProps {
  isDetailsVisible: boolean; // Remove optional
}


export const CryptoLogo = styled.img`  // Renamed from CryptoIcon
  width: 24px;
  height: 24px;
  margin-right: 12px;
`;
export const Layout = styled.div`
  display: grid;
  grid-template-columns: 350px 1fr; /* SidePanel (350px) and MainPanel (flexible) */
  grid-template-rows: auto; /* Automatically adjust rows */
  min-height: 100vh; /* Full viewport height */
  /* width: 100vw; Full viewport width */
 overflow-y: hidden;
  @media (max-width: 768px) {
    grid-template-columns: 1fr; /* Stacks SidePanel and MainPanel */
    grid-template-rows: auto 1fr; /* Adjust rows for stacking */
  }
`;
export const CryptoHeader = styled.div`
  display: flex;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 24px;
`;
export const TopBar = styled.div`
background-color: #003d82;
      color: white;
      padding: 10px 20px;
      font-size: 24px;
  display: inherit;
  align-items: center;
  justify-content: space-between;
  margin-bottom: 16px;
  font-size: 1.5em;
  font-weight: bold;      overflow-y: auto;

`;
export const SidePanel = styled.div<SidePanelProps>`
  background-color: #f5f8fa;
  height: 100vh; /* Full height */
  overflow-y: auto; /* Hides the scrollbar */
  padding: 16px;
  box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);

  @media (max-width: 768px) {
    position: fixed; /* Fix SidePanel for sliding behavior */
    z-index: 2; /* Ensure it overlays MainPanel */
    transform: translateX(${(props) => (props.isDetailsVisible ? '-100%' : '0')}); /* Slide out */
    transition: transform 0.3s ease-in-out; /* Smooth slide transition */
  }
`;
export const MainPanel = styled.div<SidePanelProps>`
  background-color: #fff;
  overflow-y: auto; /* Enable vertical scrolling */
  padding: 16px;
  box-shadow: -2px 0 4px rgba(0, 0, 0, 0.1);
-ms-overflow-style: none;
 &::-webkit-scrollbar {
    display: none; /* For Chrome, Safari, and Edge */
  }
  @media (max-width: 768px) {
    position: fixed; /* Fix MainPanel for sliding behavior */
    width: 90%; /* Full width on mobile */
    height: 100%; /* Full height on mobile */
    z-index: 3; /* Ensure it overlays SidePanel */
    transform: translateX(${(props) => (props.isDetailsVisible ? '0' : '100%')}); /* Slide in */
    transition: transform 0.3s ease-in-out; /* Smooth slide transition */
  }
`;
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
  }
`;

export const CryptoItem = styled.div`
  display: flex;
  justify-content: space-between; /* Space out the content */
  align-items: center; /* Center vertically */
  padding: 16px;
  border-bottom: 1px solid #e1e4e8;
  cursor: pointer;
  word-wrap: break-word; /* Wrap text if necessary */

  &:hover {
    background-color: #f0f0f0;
  }

  @media (max-width: 768px) {
    flex-wrap: wrap; /* Allow content to wrap on smaller screens */
    padding: 12px; /* Reduce padding for smaller screens */
  }
`;

export const CryptoInfo = styled.div`
  display: flex;
  flex-direction: column; /* Stack name and label */
  flex: 1; /* Take up remaining space */
  overflow: hidden; /* Prevent overflow */
  text-overflow: ellipsis; /* Add ellipsis for overflowing text */
  white-space: nowrap; /* Prevent wrapping */

  @media (max-width: 768px) {
    flex: none; /* Allow it to shrink */
    white-space: normal; /* Allow wrapping on mobile */
  }
`;


export const CryptoName = styled.div`
  font-weight: 500;
  margin-bottom: 4px;
`;

export const CryptoLabel = styled.div`
  color: #666;
  font-size: 12px;
`;

export const PriceInfo = styled.div`
  display: flex;
  flex-direction: column; /* Stack price and change vertically */
  text-align: right; /* Align the price to the right */
  min-width: 80px; /* Ensure some space for the price */

  @media (max-width: 768px) {
    flex-direction: row; /* Align horizontally for smaller screens */
    justify-content: space-between; /* Spread items */
    text-align: left; /* Align to the left for mobile */
    width: 100%; /* Allow it to take full width */
  }
`;

export const Price = styled.div`
  font-size: 16px;
  font-weight: 500;
  margin-bottom: 4px;
`;

export const PriceChange = styled.span<{ isPositive: boolean }>`
  color: ${({ isPositive }) => isPositive ? '#4CAF50' : '#F44336'};
  display: flex;
  align-items: center;
  gap: 4px;
  &::before {
    content: '';
    display: inline-block;
    width: 0;
    height: 0;
    border-left: 4px solid transparent;
    border-right: 4px solid transparent;
    border-bottom: ${({ isPositive }) => isPositive ? '6px solid #4CAF50' : 'none'};
    border-top: ${({ isPositive }) => !isPositive ? '6px solid #F44336' : 'none'};
  }
`;

export const BackButton = styled.button`
  background: none;
  border: none;
  color: white;
  padding: 8px;
  cursor: pointer;
  position: absolute;
  left: 24px;
  font-size: 20px;
  
  @media (min-width: 768px) {
    display: none;
  }
`;

export const ChartContainer = styled.div`
width: 100%;
height: 300px;
background: white;
padding: 16px;
border-radius: 8px;
font-size: 12px;
 margin: 16px;
padding: 16px;
background: white;
border: 1px solid #e1e4e8;
border-radius: 4px;

min-height: 300px; /* Set a minimum height */
max-height: calc(100vh - 100px); /* Keep it within the viewport vertically */

    @media (max-width: 768px) {
  width: 100%; /* Full width on mobile */
  height: auto; /* Adjust height dynamically */
  overflow-x: auto; /* Allow horizontal scrolling if necessary */
}

`;

export const DetailContainer = styled.div`
  padding: 16px;
  
`;

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
`;