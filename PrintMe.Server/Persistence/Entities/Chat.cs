namespace PrintMe.Server.Persistence.Entities;

public partial class Chat
{
    public int ChatId { get; set; }

    public int? User1Id { get; set; }

    public int? User2Id { get; set; }

    public bool? IsArchived { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual User User1 { get; set; }

    public virtual User User2 { get; set; }
}
