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
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            //StockHolderPosition
            CreateMap<DAOS.StockHolderPosition, DTOs.StockHolderPosition>();
            CreateMap<DTOs.StockHolderPosition, DAOS.StockHolderPosition>()
                .ForMember(dest => dest.StockHolderId, opt => opt.Ignore())
                .ForMember(dest => dest.StockId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.Ignore())
                .ForMember(dest => dest.StockHolder, opt => opt.Ignore());


            //StockHistory
            CreateMap<DAOS.StockHistory, DTOs.StockHistory>();
            CreateMap<DTOs.StockHistory, DAOS.StockHistory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StockId, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.Ignore());

            //StockHolder
            CreateMap<DAOS.StockHolder, DTOs.StockHolder>();
            CreateMap<DTOs.StockHolder, DAOS.StockHolder>()
                .ForMember(dest => dest.Username, opt => opt.Ignore());

            //StockPrice
            CreateMap<DAOS.StockPrice, DTOs.StockPrice>();
            CreateMap<DTOs.StockPrice, DAOS.StockPrice>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.StockId, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.Ignore());
        }
    }
}
