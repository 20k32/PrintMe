using PrintMe.Server.Models.DTOs.PrinterDto;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class GetPrinterLocationRequest
{
    public double? MaxModelHeight { get; set; }
    public double? MaxModelWidth { get; set; }
    public ICollection<PrintMaterialDto> Materials { get; set; }
}