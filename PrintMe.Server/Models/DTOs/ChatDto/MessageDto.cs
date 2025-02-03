namespace PrintMe.Server.Models.DTOs.ChatDto;

public sealed class MessageDto : INullCheck
{
    public int ChatId { get; set; }
    public string SenderId { get; init; }
    public string ReceiverId { get; init; }
    public DateTime SendedDateTime { get; init; }
    public string Payload { get; init; }

    public bool IsNull() => ChatId == default
                            || string.IsNullOrWhiteSpace(SenderId)
                            || string.IsNullOrWhiteSpace(ReceiverId)
                            || SendedDateTime == default || SendedDateTime == DateTime.MinValue
                            || string.IsNullOrWhiteSpace(Payload);
}