using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Dtos
{
    public class StockHolderPosition
    {
        public int Id { get; set; }
        public double CostBasis { get; set; }
        public int Shares { get; set; }
        public int StockId { get; set; }
        public string StockHolderId { get; set; }
    }
}
