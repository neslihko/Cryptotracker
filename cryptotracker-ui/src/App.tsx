 
import React from 'react';
import { Routes, Route, useLocation } from 'react-router-dom';
import { MobileContainer, Layout, SidePanel, MainPanel } from './styles/StyledComponents';
import CryptoList from './components/CryptoList';
import CryptoDetails from './components/CryptoDetails';

const App: React.FC = () => {
  const location = useLocation();
  const isDetailsVisible = location.pathname.startsWith('/crypto/');

  return (
    <MobileContainer>
      <Layout>
        <SidePanel $isDetailsVisible={isDetailsVisible}>
          <CryptoList />
        </SidePanel>
        <MainPanel $isDetailsVisible={isDetailsVisible}>
          <Routes>
            <Route 
              path="/crypto/:symbol" 
              element={<CryptoDetails />} 
            />
            <Route 
              path="/" 
              element={<div>Select a cryptocurrency to view details.</div>} 
            />
          </Routes>
        </MainPanel>
      </Layout>
    </MobileContainer>
  );
};

export default App;