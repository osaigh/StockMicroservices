using System;

namespace StockMicroservices.EventBus.Common.Events
{
    public class StockUpdated
    {
        public DateTime CreationDate { get; set; }

        public string Name { get; set; } = string.Empty;

        public double Change { get; set; }
    }
}
