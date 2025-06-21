using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockMicroservices.API.Models.Daos
{
    public class Stock
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public ICollection<StockHistory> StockHistories { get; set; }
    }
}
