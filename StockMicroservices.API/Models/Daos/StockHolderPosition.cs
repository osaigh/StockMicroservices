using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Daos
{
    public class StockHolderPosition
    {
        public int Id { get; set; }
        public double CostBasis { get; set; }
        public int Shares { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
        public string StockHolderId { get; set; }
        public StockHolder StockHolder { get; set; }
    }
}
