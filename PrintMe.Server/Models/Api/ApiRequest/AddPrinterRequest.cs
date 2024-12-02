using PrintMe.Server.Models.DTOs.PrinterDto;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class AddPrinterRequest
{
    public int UserId { get; set; }
    public string Description { get; set; }
    public double MinModelHeight { get; set; }
    public double MinModelWidth { get; set; }
    public double MaxModelHeight { get; set; }
    public double MaxModelWidth { get; set; }
    public double LocationX { get; set; }
    public double LocationY { get; set; }
    public ICollection<PrintMaterialDto> Materials { get; set; }
}