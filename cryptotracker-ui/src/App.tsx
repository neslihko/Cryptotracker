import React from 'react';
import {   Routes, Route, useLocation } from 'react-router-dom';
import { Layout, SidePanel, MainPanel } from './styles/StyledComponents';
import CryptoList from './components/CryptoList';
import CryptoDetails from './components/CryptoDetails';

const App: React.FC = () => {
  const location = useLocation();

  // `isDetailsVisible` is true if we are on a details route
  const isDetailsVisible = location.pathname.startsWith('/crypto/');

  return (
    <Layout>
      <SidePanel isDetailsVisible={isDetailsVisible}>
        <CryptoList />
      </SidePanel>
      <MainPanel isDetailsVisible={isDetailsVisible}>
        <Routes>
          <Route path="/crypto/:symbol" element={<CryptoDetails />} />
          <Route path="/" element={<div>Select a cryptocurrency to view details.</div>} />
        </Routes>
      </MainPanel>
    </Layout>
  );
};

export default App;