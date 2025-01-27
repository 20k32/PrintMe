using AutoMapper;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.MapProfiles
{
    public class PrintOrderMapProfile : Profile
    {
        public PrintOrderMapProfile()
        {
            CreateMap<CreateOrderRequest, PrintOrderDto>()
                .ForMember(dto => dto.StartDate,
                    options 
                        => options.MapFrom(_ => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(dto => dto.DueDate,
                    options 
                        => options.MapFrom(request => DateOnly.Parse(request.DueDate)));
            
            CreateMap<PrintOrder, PrintOrderDto>()
                .ForMember(dto => dto.StartDate,
                    options
                        => options.MapFrom(orderRaw => orderRaw.OrderDate))
                .ForMember(dto => dto.ExecutorId,
                    options => options.MapFrom(orderRaw => orderRaw.Printer != null ? orderRaw.Printer.UserId : (int?)null))
                .ReverseMap();
            
            
            CreateMap<CreateOrderRequest, PrintOrder>()
                .ForMember(orderRaw => orderRaw.OrderDate,
                    options 
                        => options.MapFrom(_ => DateOnly.FromDateTime(DateTime.Now)))
                .ForMember(orderRaw => orderRaw.DueDate,
                    options 
                        => options.MapFrom(request => DateOnly.Parse(request.DueDate)));

            CreateMap<UpdateFullOrderRequest, PrintOrder>()
                .ForMember(orderRaw => orderRaw.OrderDate,
                    options 
                        => options.MapFrom(request => DateOnly.Parse(request.StartDate)))
                .ForMember(orderRaw => orderRaw.DueDate,
                    options 
                        => options.MapFrom(request => DateOnly.Parse(request.DueDate)))
                .ReverseMap();

            CreateMap<UpdatePartialOrderRequest, PrintOrder>()
                .ForMember(orderRaw => orderRaw.DueDate,
                    options => options.MapFrom(request => 
                        !string.IsNullOrEmpty(request.DueDate) ? 
                        DateOnly.Parse(request.DueDate) : default))
                .ForMember(orderRaw => orderRaw.Price,
                    options => options.MapFrom(request => 
                        request.Price != 0 ? (decimal)request.Price : default))
                .ForMember(dest => dest.PrintOrderId, opt => opt.Ignore());
        }
    }
}