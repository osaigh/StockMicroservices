using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models.Dtos
{
    public class StockHolder
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public List<StockHolderPosition> StockHolderPositions { get; set; }

    }
}
