import React from 'react';
import styled from 'styled-components';

const InfoSection = styled.div`
  margin-top: 20px;
  padding: 20px;
  background: white;
  border-radius: 4px;
`;

const InfoGrid = styled.div`
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;
`;

const InfoItem = styled.div`
  padding: 12px;
  border-bottom: 1px solid #eee;
`;

const Label = styled.div`
  color: #666;
  font-size: 14px;
  margin-bottom: 4px;
`;

const Value = styled.div`
  font-size: 16px;
  font-weight: 500;
`;

interface AdditionalInfoProps {
  marketCap: number;
  volume24h: number;
  supply: number;
  maxSupply: number | null;
  vwap24h: number;
  rank: number;
}

export const AdditionalInfo: React.FC<AdditionalInfoProps> = ({
  marketCap,
  volume24h,
  supply,
  maxSupply,
  vwap24h,
  rank
}) => {
  const formatNumber = (num: number) => num.toLocaleString();
  const formatUSD = (num: number) => `$${formatNumber(num)}`;

  return (
    <InfoSection>
      <h3>Zusätzliche Informationen</h3>
      <InfoGrid>
        <InfoItem>
          <Label>Marktkapitalisierung</Label>
          <Value>{formatUSD(marketCap)}</Value>
        </InfoItem>
        <InfoItem>
          <Label>24h Volumen</Label>
          <Value>{formatUSD(volume24h)}</Value>
        </InfoItem>
        <InfoItem>
          <Label>Verfügbares Angebot</Label>
          <Value>{formatNumber(supply)}</Value>
        </InfoItem>
        <InfoItem>
          <Label>Maximales Angebot</Label>
          <Value>{maxSupply ? formatNumber(maxSupply) : 'Unbegrenzt'}</Value>
        </InfoItem>
        <InfoItem>
          <Label>24h VWAP</Label>
          <Value>{formatUSD(vwap24h)}</Value>
        </InfoItem>
        <InfoItem>
          <Label>Marktrang</Label>
          <Value>#{rank}</Value>
        </InfoItem>
      </InfoGrid>
    </InfoSection>
  );
};