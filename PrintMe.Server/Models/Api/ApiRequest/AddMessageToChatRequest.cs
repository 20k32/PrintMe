using PrintMe.Server.Models.DTOs.ChatDto;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class AddMessageToChatRequest(int chatId, MessageDto messageDto) : INullCheck
{
    public int ChatId { get; init; } = chatId;
    public MessageDto Message { get; init; } = messageDto;

    public bool IsNull() => ChatId == default || Message.IsNull();
}

public class SendMessageToChatRequest(int chatId, DateTime sendedDateTime, string payload) : INullCheck
{
    public int ChatId { get; init; } = chatId;

    public DateTime SendedDateTime { get; init; } = sendedDateTime;

    public string Payload { get; init; } = payload;

    public bool IsNull() => ChatId == default 
                            || SendedDateTime == default || SendedDateTime == DateTime.MinValue
                            || string.IsNullOrWhiteSpace(Payload);
}