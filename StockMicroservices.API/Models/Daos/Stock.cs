using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Daos
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }
        public ICollection<StockHistory> StockHistories { get; set; }
    }
}
