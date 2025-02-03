using AutoMapper;
using Microsoft.Extensions.Options;
using PrintMe.Server.Models.DTOs.ChatDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.MapProfiles;

public class ChatMapProfile : Profile
{
    public ChatMapProfile()
    {
        CreateMap<Chat, ChatDto>()
            .ForMember(dest => dest.Id,
                options
                    => options.MapFrom(source => source.ChatId))
            .ForMember(dest => dest.IsArchived,
                options
                    => options.MapFrom(source => source.IsArchived))
            .ForMember(dest => dest.User1Id,
                options
                    => options.MapFrom(source => source.User1Id))
            .ForMember(dest => dest.User2Id,
                options
                    => options.MapFrom(source => source.User2Id))
            .ReverseMap();

        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.ChatId,
                options
                    => options.MapFrom(source => source.ChatId))
            .ForMember(dest => dest.ChatId,
                options
                    => options.MapFrom(source => source.ChatId))
            .ForMember(dest => dest.Payload,
                options
                    => options.MapFrom(source => source.Text))
            .ForMember(dest => dest.SendedDateTime,
                options
                    => options.MapFrom(source => source.SendTime))
            .ReverseMap();
    }
}