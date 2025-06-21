export default interface StockOrder{
    id: number,
    stock: string,
    stockId: number,
    shares: number,
    timeInForce: string,
    transactionType: string,
    orderType: string,
    stopLimitPrice: number
}