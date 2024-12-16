export interface Crypto {
        symbol:string,
    name:string,
    rank: number,
    currentPrice: number,
    volumeUsd24Hr: number,
     marketCap: number,
     changePercent24Hr: number,
     supply: number,
     maxSupply: number,
     vwaP24Hr: number,
     lastUpdated:string  ,
     logoUrl: string;
}
export interface PriceHistory {
    timestamp: string;
    price: number;
    volume: number; // Retained the additional `volume` field

}