namespace PrintMe.Server.Persistence.Entities;

public partial class RequestStatus
{
    public int RequestStatusId { get; set; }

    public string Status { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
