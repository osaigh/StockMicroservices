using System.ComponentModel.DataAnnotations;

namespace StockMicroservices.API.Models.Daos
{
    public class StockPosition
    {
        [Key]
        public int Id { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public int Shares { get; set; }
        public double CurrentPrice { get; set; }
        public double CostBasis { get; set; }
    }
}
