namespace PrintMe.Server.Persistence.Entities;

public partial class PrintOrderStatus
{
    public int PrintOrderStatusId { get; set; }

    public string Status { get; set; }

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();
}
