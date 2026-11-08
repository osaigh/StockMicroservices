using System;

namespace StockMicroservices.API.Models.Daos
{
    public class StockHistory
    {
        public string StockId { get; set; } = string.Empty;

        public DateTimeOffset Date { get; set; }

        public double Price { get; set; }
    }
}
