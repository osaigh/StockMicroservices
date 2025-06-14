using System;
using System.Collections.Generic;
using System.Text;

namespace StockMicroservices.EventBus.Common.Events
{
    public class StockUpdated
    {
        public DateTime CreationDate { get; set; }
        public string Name { get; set; }
        public double Change { get; set; }
    }
}
