using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Daos
{
    public class StockHolder
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<StockHolderPosition> StockHolderPositions { get; set; }

    }
}
