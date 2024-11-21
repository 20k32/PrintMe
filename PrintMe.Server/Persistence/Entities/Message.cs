namespace PrintMe.Server.Persistence.Entities;

public partial class Message
{
    public int ChatId { get; set; }

    public DateTime SendTime { get; set; }

    public int? SenderId { get; set; }

    public string Text { get; set; }

    public virtual Chat Chat { get; set; }

    public virtual User Sender { get; set; }
}
