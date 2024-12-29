using AutoMapper;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.MapProfiles
{
    public class PrintMaterialMapProfile : Profile
    {
        public PrintMaterialMapProfile()
        {
            CreateMap<PrintMaterial, PrintMaterialDto>()
                .ForMember(dto => dto.PrintMaterialId,
                    options =>
                        options.MapFrom(source => source.PrintMaterialId));
        }
        
    }
}