namespace PrintMe.Server.Models.DTOs.ChatDto;

public sealed class MessageDto : INullCheck
{
    public int ChatId { get; set; }
    public int SenderId { get; set; }
    public DateTime SentDateTime { get; set; }
    public string Payload { get; set; }

    public bool IsNull() => ChatId == default
                            || SenderId == default
                            || SentDateTime == default || SentDateTime == DateTime.MinValue
                            || string.IsNullOrWhiteSpace(Payload);

    public MessageDto()
    {
    }

    public MessageDto(int chatId, int senderId, DateTime sentDateTime, string payload)
    {
        ChatId = chatId;
        SenderId = senderId;
        SentDateTime = sentDateTime;
        Payload = payload;
    }
}