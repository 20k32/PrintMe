using AutoMapper;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.MapProfiles
{
    public class PrinterMapProfile : Profile
    {
        public PrinterMapProfile()
        {
            CreateMap<PrinterDto, Printer>()
                .ForMember(source => source.PrinterModel,
                    options
                        => options.MapFrom(dto => new PrinterModel() {Name = dto.ModelName}))
                .ForMember(source => source.Materials,
                    options 
                        => options.MapFrom<ICollection<PrintMaterial>>(_ => null));
        }
    }
}