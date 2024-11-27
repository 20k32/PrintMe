namespace PrintMe.Server.Persistence.Entities
{
    public class SimplePrinter
    {
        public int PrinterId { get; set; }
        public PrinterModel PrinterModel { get; set; }
        public ICollection<PrintMaterial> Materials { get; set; } = new List<PrintMaterial>();
    }
}