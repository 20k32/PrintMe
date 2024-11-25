using AutoMapper;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs;

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
    public int RequestStatusId { get; init; }
    public int? RequestStatusReasonId { get; init; }

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
    }
}