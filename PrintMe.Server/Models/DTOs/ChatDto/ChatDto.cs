namespace PrintMe.Server.Models.DTOs.ChatDto;

public class ChatDto
{
    public int Id { get; set; }
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public bool IsArchived { get; set; }
    
    public ChatDto(int id, int user1Id, int user2Id, bool shouldArchive)
    {
        Id = id;
        User1Id = user1Id;
        User2Id = user2Id;
        IsArchived = shouldArchive;
    }

    public ChatDto()
    { }
}