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

            //StockHolderPosition
            CreateMap<DAOS.StockHolderPosition, DTOs.StockHolderPosition>();
            CreateMap<DTOs.StockHolderPosition, DAOS.StockHolderPosition>();


            //StockHistory
            CreateMap<DAOS.StockHistory, DTOs.StockHistory>();
            CreateMap<DTOs.StockHistory, DAOS.StockHistory>();


            //StockHolder
            CreateMap<DAOS.StockHolder, DTOs.StockHolder>();
            CreateMap<DTOs.StockHolder, DAOS.StockHolder>();


        }
    }
}
