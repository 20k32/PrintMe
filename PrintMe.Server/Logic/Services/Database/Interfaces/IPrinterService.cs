using PrintMe.Server.Models.DTOs.PrinterDto;

namespace PrintMe.Server.Logic.Services.Database.Interfaces;

public interface IPrinterService
{
    IAsyncEnumerable<SimplePrinterDto> GetPrintersBasicAsync(int skip, int take);
    IAsyncEnumerable<PrinterDto> GetPrintersDetailedAsync(int skip, int take);
    Task<SimplePrinterDto> GetPrinterBasicByIdAsync(int id);
    Task<PrinterDto> GetPrinterDetailedByIdAsync(int id);
    Task<IEnumerable<PrinterDto>> GetPrintersDetailedByUserId(int id, bool? isDeactivated = null);
    Task<IEnumerable<SimplePrinterDto>> GetPrintersBasicByUserId(int id);
    Task AddPrinterAsync(PrinterDto printerDto);

    IAsyncEnumerable<PrinterLocationDto> GetPrinterLocationAsync(ICollection<PrintMaterialDto> material,
        double? maxHeight, double? maxWidth);

    Task<List<PrintMaterialDto>> GetMaterialsAsync();
    Task<List<PrinterModelDto>> GetModelsAsync();
    Task DeactivatePrinterAsync(int printerId);
    Task ActivatePrinterAsync(int printerId);
    // Task GetPrinterModelByName(string name); //private
}