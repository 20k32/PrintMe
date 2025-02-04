using System.Text.Json.Serialization;
using PrintMe.Server.Models.DTOs.PrinterDto;

namespace PrintMe.Server.Models.Api.ApiRequest;

public class EditPrinterRequest
{
    [JsonIgnore]
    public int UserId { get; set; }
    public int PrinterID { get; set; }
    public string Description { get; set; }
    public ICollection<PrintMaterialDto> Materials { get; set; }
}