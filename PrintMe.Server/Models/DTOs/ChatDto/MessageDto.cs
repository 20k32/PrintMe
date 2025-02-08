namespace PrintMe.Server.Models.DTOs.ChatDto;

public sealed class MessageDto(int chatId = default, string senderId = default, DateTime sendedDateTime = default, string payload = default) : INullCheck
{
    public int ChatId { get; init; } = chatId;
    public string SenderId { get; init; } = senderId;
    public DateTime SendedDateTime { get; init; } = sendedDateTime;
    public string Payload { get; init; } = payload;

    public bool IsNull() => ChatId == default
                            || string.IsNullOrWhiteSpace(SenderId)
                            || SendedDateTime == default || SendedDateTime == DateTime.MinValue
                            || string.IsNullOrWhiteSpace(Payload);
}