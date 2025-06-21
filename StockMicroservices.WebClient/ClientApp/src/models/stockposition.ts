export default interface StockPosition{
    id: number,
    name: string,
    stockId: number,
    shares: number,
    costBasis: number,
    currentPrice: number,
    marketValue:number,
    gainLossPercent: number
}