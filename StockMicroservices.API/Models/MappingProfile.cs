using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAOS = StockMicroservices.API.Models.Daos;
using DTOs = StockMicroservices.API.Models.Dtos;

namespace StockMicroservices.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Stocks
            CreateMap<DAOS.Stock, DTOs.Stock>();
            CreateMap<DTOs.Stock, DAOS.Stock>();


            //StockHistory
            CreateMap<DAOS.StockHistory, DTOs.StockHistory>();
            CreateMap<DTOs.StockHistory, DAOS.StockHistory>();



        }
    }
}
