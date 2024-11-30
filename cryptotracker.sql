
  -- *id: Unique identifier.
  --  * name: Name of the cryptocurrency.
  --  * symbol: Cryptocurrency symbol.
  --  * date: Date of the price record.
  --  -- * price_usd: Price in USD.
  --  * volume_usd: Trading volume in USD.
  --  * market_cap_usd: Market capitalization in USD.
   
CREATE TABLE Cryptocurrencies (
    id SERIAL PRIMARY KEY,
    symbol VARCHAR(10) UNIQUE,
    name VARCHAR(100),
    current_price_usd DECIMAL(18,8),
    volume_usd DECIMAL(18,2),
    market_cap_usd DECIMAL(18,2),
    last_updated TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE PriceHistory (
    id SERIAL PRIMARY KEY,
    crypto_id INTEGER REFERENCES Cryptocurrencies(id),
    price_usd DECIMAL(18,8),
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Create basic indexes
CREATE INDEX idx_crypto_symbol ON Cryptocurrencies(symbol);
CREATE INDEX idx_price_timestamp ON PriceHistory(timestamp);


-- Insert some test data
-- INSERT INTO Cryptocurrencies (symbol, name, current_price_usd, volume_usd, market_cap_usd, last_updated)
-- VALUES ('BTC', 'Bitcoin', 50000.00, 2000000000.00, 900000000000.00, CURRENT_TIMESTAMP);


-- INSERT INTO PriceHistory (crypto_id, price_usd, timestamp)
-- VALUES (4, 48000.00, '2024-11-19 10:00:00');


-- SELECT 
--     c.name, c.symbol, c.current_price_usd, ph.price_usd, ph.timestamp
-- FROM 
--     Cryptocurrencies c
-- JOIN 
--     PriceHistory ph ON c.id = ph.crypto_id
-- WHERE 
--     c.symbol = 'BTC'
-- ORDER BY 
--     ph.timestamp DESC;


-- select * from Cryptocurrencies
-- select * from PriceHistory
 