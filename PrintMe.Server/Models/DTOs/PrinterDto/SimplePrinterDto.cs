namespace PrintMe.Server.Models.DTOs.PrinterDto
{
    public class SimplePrinterDto
    {
        public int Id { get; set; }
        public string ModelName { get; set; }
        public ICollection<PrintMaterialDto> Materials { get; set; } 
    }
}