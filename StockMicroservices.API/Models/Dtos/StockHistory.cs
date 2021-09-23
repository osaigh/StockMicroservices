using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Dtos
{
    public class StockHistory
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Price { get; set; }
        public int StockId { get; set; }
    }
}
