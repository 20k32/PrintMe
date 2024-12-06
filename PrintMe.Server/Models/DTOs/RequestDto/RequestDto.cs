using AutoMapper;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs.RequestDto;

public class RequestDto : INullCheck
{
    public int RequestId { get; init; }
    public int UserId { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public string UserTextData { get; init; }
    public int UserSenderId { get; init; }
    public int RequestTypeId { get; init; }
    public int? ReportedUserId { get; init; }
    public int? DeleteUserId { get; init; }
    public int? ModelId { get; init; }
    public double? LocationX { get; init; }
    public double? LocationY { get; init; }
    public double? MinModelHeight { get; init; }
    public double? MinModelWidth { get; init; }
    public double? MaxModelHeight { get; init; }
    public double? MaxModelWidth { get; init; }
    public int RequestStatusId { get; set; }
    public int? RequestStatusReasonId { get; set; }

    public bool IsNull() => RequestId == default
                            || UserId == default
                            || string.IsNullOrWhiteSpace(Title)
                            || string.IsNullOrWhiteSpace(Description);
}

public class RequestProfile : Profile
{
    public RequestProfile()
    {
        CreateMap<Request, RequestDto>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserSenderId))
            .ReverseMap();

        CreateMap<AddPrinterRequest, RequestDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => "Adding printer request"))
            .ForMember(dest => dest.UserTextData, opt => opt.MapFrom(src => string.Join(", ", src.Materials.Select(m => $"{m.PrintMaterialId}:{m.Name}"))))
            .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(src => 2))
            .ForMember(dest => dest.RequestStatusId, opt => opt.MapFrom(src => 1));

        CreateMap<EditPrinterRequest, RequestDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => "Editing printer request"))
            .ForMember(dest => dest.UserTextData, opt => opt.MapFrom(src => string.Join(", ", src.Materials.Select(m => $"{m.PrintMaterialId}:{m.Name}"))))
            .ForMember(dest => dest.RequestTypeId, opt => opt.MapFrom(src => 4))
            .ForMember(dest => dest.RequestStatusId, opt => opt.MapFrom(src => 1))
            .ForMember(dest => dest.ModelId, opt => opt.MapFrom(src => src.PrinterID));

        CreateMap<Request, PrintMe.Server.Models.DTOs.PrinterDto.PrinterDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ModelId))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.LocationX, opt => opt.MapFrom(src => src.LocationX))
            .ForMember(dest => dest.LocationY, opt => opt.MapFrom(src => src.LocationY))
            .ForMember(dest => dest.MaxModelHeight, opt => opt.MapFrom(src => src.MaxModelHeight))
            .ForMember(dest => dest.MinModelHeight, opt => opt.MapFrom(src => src.MinModelHeight))
            .ForMember(dest => dest.MaxModelWidth, opt => opt.MapFrom(src => src.MaxModelWidth))
            .ForMember(dest => dest.MinModelWidth, opt => opt.MapFrom(src => src.MinModelWidth))
            .ForMember(dest => dest.Materials, opt => opt.Ignore())
            .ForMember(dest => dest.ModelName, opt => opt.MapFrom(src => src.UserTextData.Split(new[] { ',' })[0]));
    }
}