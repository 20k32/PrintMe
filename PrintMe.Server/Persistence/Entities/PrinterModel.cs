namespace PrintMe.Server.Persistence.Entities;

public partial class PrinterModel
{
    public int PrinterModelId { get; set; }

    public string Name { get; set; }

    public virtual ICollection<Printer> Printers { get; set; } = new List<Printer>();
}
