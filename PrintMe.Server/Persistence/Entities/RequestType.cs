namespace PrintMe.Server.Persistence.Entities;

public partial class RequestType
{
    public int RequestTypeId { get; set; }

    public string Type { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
