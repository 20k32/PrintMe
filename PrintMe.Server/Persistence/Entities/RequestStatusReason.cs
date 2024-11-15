namespace PrintMe.Server.Persistence.Entities;

public partial class RequestStatusReason
{
    public int RequestStatusReasonId { get; set; }

    public string Reason { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
