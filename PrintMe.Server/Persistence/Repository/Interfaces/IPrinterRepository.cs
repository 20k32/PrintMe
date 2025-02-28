using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IPrinterRepository
{
    IAsyncEnumerable<SimplePrinter> GetSimplePrinterStreamAsync(int skip, int take);
    IAsyncEnumerable<Printer> GetPrinterStreamAsync(int skip, int take);
    Task<SimplePrinter> GetPrinterBasicInfoByIdAsync(int id);
    Task<Printer> GetPrinterDetailedInfoAsync(int id);
    Task<List<SimplePrinter>> GetPrintersBasicInfoForUserAsync(int userId);
    Task<List<Printer>> GetPrintersForUserAsync(int userId);
    Task<List<Printer>> GetPrintersForUserAsync(int userId, bool? isDeactivated);
    Task DeactivatePrinterAsync(int printerId);
    Task AddPrinterAsync(Printer printer);

    IAsyncEnumerable<PrinterLocationDto> GetPrinterLocationAsync(ICollection<PrintMaterialDto> material,
        double? maxHeight, double? maxWidth);

    Task<List<PrintMaterial>> GetAllMaterialsAsync();
    Task<List<PrinterModel>> GetAllModelsAsync();
    Task<PrinterModel> GetModelByNameAsync(string name);
    Task<PrinterModel> GetModelByIdAsync(int id);
    Task<PrintMaterial> GetMaterialByIdAsync(int printMaterialId);
    Task ActivatePrinterAsync(int printerId);
}