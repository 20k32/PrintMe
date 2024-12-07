using AutoMapper;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.DTOs.RequestDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.MapProfiles
{
    public class RequestMapProfile : Profile
    {
        public RequestMapProfile()
        {
            CreateMap<Request, RequestDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserSenderId))
                .ReverseMap();
        }
    }
}