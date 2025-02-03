using PrintMe.Server.Models.DTOs.ChatDto;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class AddMessageToChatRequest(int chatId, MessageDto messageDto) : INullCheck
{
    public int ChatId { get; init; } = chatId;
    public MessageDto Message { get; init; } = messageDto;

    public bool IsNull() => ChatId == default || Message.IsNull();
}