using System;
using System.Collections.Generic;
using System.Linq;
using DAO = StockMicroservices.API.Models.Daos;
using DTO = StockMicroservices.API.Models.Dtos;

namespace StockMicroservices.API.Models
{
    public static class MappingExtensions
    {
        public static DTO.Stock ToDto(this DAO.Stock stock)
        {
            return new DTO.Stock
            {
                Id = stock.Id,
                Name = stock.Name,
                Price = Math.Round(stock.Price, 2),
                Volume = stock.Volume,
                StockHistories = stock.StockHistories.Select(history => history.ToDto()).ToList(),
            };
        }

        public static DTO.StockHistory ToDto(this DAO.StockHistory history)
        {
            return new DTO.StockHistory
            {
                Date = history.Date,
                Price = history.Price,
            };
        }

        public static List<DTO.Stock> ToDtoList(this IEnumerable<DAO.Stock> stocks)
        {
            return stocks.Select(stock => stock.ToDto()).ToList();
        }
    }
}
