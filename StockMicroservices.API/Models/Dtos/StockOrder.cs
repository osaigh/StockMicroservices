namespace StockMicroservices.API.Models.Dtos
{
    public class StockOrder
    {
        public int Id { get; set; }
        public string Stock {  get; set; }
        public int StockId { get; set; }
        public double Shares { get; set; }
        public TimeInForce TimeInForce { get; set; }
        public TransactionType TransactionType { get; set; }
        public OrderType OrderType { get; set; }
        public double StopLimitPrice { get; set; }

    }
}
