using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Dtos
{
    public class StockPrice
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }
        public int StockId { get; set; }
    }
}
