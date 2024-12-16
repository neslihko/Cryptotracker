import React from 'react';

interface SearchBarProps {
  value: string;
  onChange: (value: string) => void;
}

const SearchBar: React.FC<SearchBarProps> = ({ value, onChange }) => (
  <input
    type="text"
    placeholder="Search cryptocurrencies..."
    value={value}
    onChange={(e) => onChange(e.target.value)}
    style={{ padding: '8px', width: '100%' }}
  />
);

export default SearchBar;
