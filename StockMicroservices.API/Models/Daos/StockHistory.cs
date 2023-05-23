using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Daos
{
    public class StockHistory
    {
        public DateTimeOffset Date { get; set; }
        public double Price { get; set; }
    }
}
