using System.ComponentModel.DataAnnotations;

namespace StockMicroservices.API.Models.Daos
{
    public class StockOrder
    {
        [Key]
        public int Id { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public double Shares { get; set; }
        public TimeInForce TimeInForce { get; set; }
        public TransactionType TransactionType { get; set; }
        public OrderType OrderType { get; set; }
        public double StopLimitPrice { get; set; }
    }
}
