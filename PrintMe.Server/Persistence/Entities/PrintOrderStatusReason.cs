namespace PrintMe.Server.Persistence.Entities;

public partial class PrintOrderStatusReason
{
    public int PrintOrderStatusReasonId { get; set; }

    public string Reason { get; set; }

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();
}
