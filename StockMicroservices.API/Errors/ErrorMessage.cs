using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Errors
{
    public class ErrorMessage
    {
        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}
