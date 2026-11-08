using System;
using System.Collections.Generic;

namespace StockMicroservices.API.Models.Dtos
{
    public class Stock
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public double Price { get; set; }

        public int Volume { get; set; }

        public List<StockHistory> StockHistories { get; set; } = new List<StockHistory>();
    }
}
