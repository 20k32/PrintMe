using System.Text.Json.Serialization;

namespace PrintMe.Server.Models.DTOs.PrinterDto
{
    public class PrintMaterialDto
    {
        public int PrintMaterialId { get; set; }

        [JsonIgnore]
        public string Name { get; set; }
    }
}