using System;

namespace StockMicroservices.API.Models.Dtos
{
    public class StockHistory
    {
        public DateTimeOffset Date { get; set; }

        public double Price { get; set; }
    }
}
