namespace PrintMe.Server.Persistence.Entities;

public partial class PrintMaterial
{
    public int PrintMaterialId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();

    public virtual ICollection<Printer> Printers { get; set; } = new List<Printer>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
