using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockMicroservices.API.Services
{
    public interface IStockUpdateListener
    {
        void StartListener();
    }
}
