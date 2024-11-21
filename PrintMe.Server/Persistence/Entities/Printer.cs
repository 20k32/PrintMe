namespace PrintMe.Server.Persistence.Entities;

public partial class Printer
{
    public int PrinterId { get; set; }

    public int? PrinterModelId { get; set; }

    public int? UserId { get; set; }

    public string Description { get; set; }

    public double MinModelHeight { get; set; }

    public double MinModelWidth { get; set; }

    public double MaxModelHeight { get; set; }

    public double MaxModelWidth { get; set; }

    public double LocationX { get; set; }

    public double LocationY { get; set; }

    public virtual ICollection<PrintOrder> PrintOrders { get; set; } = new List<PrintOrder>();

    public virtual PrinterModel PrinterModel { get; set; }

    public virtual User User { get; set; }

    public virtual ICollection<PrintMaterial> Materials { get; set; } = new List<PrintMaterial>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
