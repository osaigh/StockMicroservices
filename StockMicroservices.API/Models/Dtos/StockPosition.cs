using System;

namespace StockMicroservices.API.Models.Dtos
{
    public class StockPosition
    {
        public int Id { get; set; }
        public double CostBasis { get; set; }
        public int Shares { get; set; }
        public string Name { get; set; }
        public int StockId { get; set; }
        public double CurrentPrice { get; set; }
        public double MarketValue { get { return Math.Round(Shares * CurrentPrice, 2); } }
        public double GainLossPercent { get { return Math.Round(((CurrentPrice * Shares - CostBasis) * 100 / CostBasis), 2); } }
    }
}
