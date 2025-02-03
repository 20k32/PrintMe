namespace PrintMe.Server.Models.DTOs.ChatDto;

public class ChatDto
{
    public int Id { get; init; }
    public int User1Id { get; init; }
    public int User2Id { get; init; }
    public bool IsArchived { get; init; }
}