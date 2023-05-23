using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Daos
{
    public class StockHolderPosition
    {
        public double CostBasis { get; set; }
        public int Shares { get; set; }
    }
}
