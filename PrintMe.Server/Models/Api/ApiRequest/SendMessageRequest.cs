namespace PrintMe.Server.Models.Api.ApiRequest;

public class SendMessageRequest
{
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Payload { get; set; }
    public DateTime SentDate { get; set; }
}