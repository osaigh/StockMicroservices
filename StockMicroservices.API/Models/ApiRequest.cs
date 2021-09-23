using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Models
{
    public class ApiRequest
    {
        public string RequestId { get; set; }
        public string JsonString { get; set; }
    }
}
