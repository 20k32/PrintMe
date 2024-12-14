using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    internal sealed class PrinterRepository
    {
        private readonly PrintMeDbContext _dbContext;

        public PrinterRepository(PrintMeDbContext dbContext) => _dbContext = dbContext;
        public async IAsyncEnumerable<SimplePrinter> GetSimplePrinterStreamAsync(int skip, int take)
        {
            await foreach (var printerRaw in _dbContext.Printers
                               .AsNoTracking()
                               .Select(printer => new SimplePrinter()
                               {
                                   Materials = printer.Materials,
                                   PrinterId = printer.PrinterId,
                                   PrinterModel = printer.PrinterModel
                               })
                               .OrderBy(printer => printer.PrinterId)
                               .Skip(skip)
                               .Take(take)
                               .AsAsyncEnumerable())
            {
                yield return printerRaw;
            }
        }
        
        public async IAsyncEnumerable<Printer> GetPrinterStreamAsync(int skip, int take)
        {
            await foreach (var printerRaw in _dbContext.Printers
                               .AsNoTracking()
                               .Include(printer => printer.PrinterModel)
                               .Include(printer => printer.Materials)
                               .OrderBy(printer => printer.PrinterId)
                               .Skip(skip)
                               .Take(take)
                               .AsAsyncEnumerable())
            {
                yield return printerRaw;
            }
        }

        public async Task<SimplePrinter> GetPrinterBasicInfoByIdAsync(int id) =>
            await _dbContext.Printers
                .AsNoTracking()
                .Select(printer => new SimplePrinter()
                {
                    Materials = printer.Materials,
                    PrinterId = printer.PrinterId,
                    PrinterModel = printer.PrinterModel
                })
                .FirstOrDefaultAsync(printer => printer.PrinterId == id);

        public async Task<Printer> GetPrinterDetailedInfoAsync(int id) =>
            await _dbContext.Printers
                .AsNoTracking()
                .Include(printer => printer.PrinterModel)
                .Include(printer => printer.Materials)
                .FirstOrDefaultAsync(printer => printer.PrinterId == id);

        public Task<List<SimplePrinter>> GetPrintersBasicInfoForUserAsync(int userId) =>
            _dbContext.Printers
                .AsNoTracking()
                .Select(printer => new SimpleUserPrinter()
            {
                Materials = printer.Materials,
                PrinterId = printer.PrinterId,
                PrinterModel = printer.PrinterModel,
                UserId = printer.UserId.Value
            }).Where(simplePrinter => simplePrinter.UserId == userId)
                .Select(simpleUserPrinter => new SimplePrinter()
                {
                    Materials = simpleUserPrinter.Materials,
                    PrinterId = simpleUserPrinter.PrinterId,
                    PrinterModel = simpleUserPrinter.PrinterModel
                })
                .ToListAsync();
        
        public Task<List<Printer>> GetPrintersForUserAsync(int userId) =>
            _dbContext.Printers
                .AsNoTracking()
                .Include(printer => printer.PrinterModel)
                .Include(printer => printer.Materials)
                .Where(printer => printer.UserId == userId).ToListAsync();

        public async Task AddPrinterAsync(Printer printer)
        {
            var existingEntity = await _dbContext.Printers.FindAsync(printer.PrinterId);
            if (existingEntity != null)
            {
                _dbContext.Entry(existingEntity).State = EntityState.Detached;
            }

            await _dbContext.Printers.AddAsync(printer);
            await _dbContext.SaveChangesAsync();
        }
        
        public async IAsyncEnumerable<PrinterLocationDto> GetPrinterLocationAsync(ICollection<PrintMaterialDto> material, double maxHeight, double maxWidth)
        {
            await foreach (var printerRaw in _dbContext.Printers
                               .AsNoTracking()
                               .Where(printer => printer.Materials.Any(material => printer.Materials.Select(m => m.PrintMaterialId).Contains(material.PrintMaterialId)))
                               .Where(printer => printer.MaxModelHeight >= maxHeight && printer.MaxModelWidth >= maxWidth)
                               .Select(printer => new PrinterLocationDto()
                               {
                                   Id = printer.PrinterId,
                                   LocationX = printer.LocationX,
                                   LocationY = printer.LocationY
                               })
                               .OrderBy(printer => printer.Id)
                               .AsAsyncEnumerable())
            {
                yield return printerRaw;
            }
        }
    }
}