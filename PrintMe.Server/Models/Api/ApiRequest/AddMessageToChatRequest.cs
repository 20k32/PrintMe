using PrintMe.Server.Models.DTOs.ChatDto;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class AddMessageToChatRequest(int chatId, MessageDto messageDto) : INullCheck
{
    public int ChatId { get; init; } = chatId;
    public MessageDto Message { get; init; } = messageDto;

    public bool IsNull() => ChatId == default || Message.IsNull();
}

public class SendMessageToChatRequest(int chatId, DateTime sentDateTime, string payload) : INullCheck
{
    public int ChatId { get; init; } = chatId;

    public DateTime SentDateTime { get; init; } = sentDateTime;

    public string Payload { get; init; } = payload;

    public bool IsNull() => ChatId == default 
                            || SentDateTime == default || SentDateTime == DateTime.MinValue
                            || string.IsNullOrWhiteSpace(Payload);
}