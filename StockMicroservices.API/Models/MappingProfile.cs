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
            CreateMap<DTOs.Stock, DAOS.Stock>()
                .ForMember(s => s.StockHistories, opt => opt.Ignore());


            //StockHistory
            CreateMap<DAOS.StockHistory, DTOs.StockHistory>()
                .ForMember(s => s.Stock, opt => opt.MapFrom(s => s.Stock.Name));
            CreateMap<DTOs.StockHistory, DAOS.StockHistory>()
                .ForMember(s => s.Stock, opt => opt.Ignore());


            //StockOrder
            CreateMap<DAOS.StockOrder, DTOs.StockOrder>()
                .ForMember(s => s.Stock, opt => opt.MapFrom(s => s.Stock.Name));
            CreateMap<DTOs.StockOrder, DAOS.StockOrder>()
                .ForMember(s => s.Stock, opt => opt.Ignore());

            //StockPosition
            CreateMap<DAOS.StockPosition, DTOs.StockPosition>()
                .ForMember(s => s.Name, opt => opt.MapFrom(s => s.Stock.Name));
            CreateMap<DTOs.StockPosition, DAOS.StockPosition>()
                .ForMember(s => s.Stock, opt => opt.Ignore());

        }
    }
}
