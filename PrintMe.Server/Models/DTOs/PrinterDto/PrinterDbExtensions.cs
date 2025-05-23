using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Models.DTOs.PrinterDto
{
    public static class PrinterDbExtensions
    {
        public static PrintMaterialDto MapToDto(this PrintMaterial material) =>
            new()
            {
                Name = material.Name,
                PrintMaterialId = material.PrintMaterialId
            };

        public static PrinterModelDto MapToDto(this PrinterModel model) =>
            new()
            {
                PrinterModelId = model.PrinterModelId,
                Name = model.Name
            };

        public static ICollection<PrintMaterialDto> MapToDtos(this ICollection<PrintMaterial> materials) 
            => materials.Select(materialRaw => materialRaw.MapToDto()).ToList();
        
        public static PrinterDto MapToDto(this Printer printer) =>
            new()
            {
                Id = printer.PrinterId,
                Description = printer.Description,
                LocationX = printer.LocationX,
                LocationY = printer.LocationY,
                MaxModelHeight = printer.MaxModelHeight,
                MinModelHeight = printer.MinModelHeight,
                MaxModelWidth = printer.MaxModelWidth,
                MinModelWidth = printer.MinModelWidth,
                Materials = printer.Materials?.MapToDtos() ?? Enumerable.Empty<PrintMaterialDto>().ToList(),
                ModelName = printer.PrinterModel.Name,
                IsDeactivated = printer.IsDeactivated,
                UserId = printer.UserId ?? 0
            };
        
        public static SimplePrinterDto MapToDto(this SimplePrinter simplePrinter) =>
            new()
            {
                Id = simplePrinter.PrinterId,
                Materials = simplePrinter.Materials.MapToDtos(),
                ModelName = simplePrinter.PrinterModel.Name
            };

        public static ICollection<SimplePrinterDto>  MapToDtos(this ICollection<SimplePrinter> simplePrinters) 
            => simplePrinters.Select(simplePrinter => simplePrinter.MapToDto()).ToList();
        
        public static ICollection<PrinterDto>  MapToDtos(this ICollection<Printer> printers) 
            => printers.Select(printer => printer.MapToDto()).ToList();
        

        public static SimplePrinterDto MapToSimpleDto(this Printer printer) =>
            new()
            {
                Id = printer.PrinterId,
                Materials = printer.Materials.MapToDtos(),
                ModelName = printer.PrinterModel.Name
            };
        
        

        public static Printer MapToEntity(this PrinterDto printerDto) =>
            new()
            {
                PrinterId = printerDto.Id,
                Description = printerDto.Description,
                LocationX = printerDto.LocationX,
                LocationY = printerDto.LocationY,
                MaxModelHeight = printerDto.MaxModelHeight,
                MinModelHeight = printerDto.MinModelHeight,
                MaxModelWidth = printerDto.MaxModelWidth,
                MinModelWidth = printerDto.MinModelWidth,
                PrinterModel = new PrinterModel
                {
                    Name = printerDto.ModelName
                }
            };
    }
}