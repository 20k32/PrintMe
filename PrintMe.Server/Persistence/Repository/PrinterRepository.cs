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

        public Task<SimplePrinter> GetPrinterBasicInfoByIdAsync(int id) =>
            _dbContext.Printers
                .AsNoTracking()
                .Select(printer => new SimplePrinter()
                {
                    Materials = printer.Materials,
                    PrinterId = printer.PrinterId,
                    PrinterModel = printer.PrinterModel
                })
                .FirstOrDefaultAsync(printer => printer.PrinterId == id);

        public Task<Printer> GetPrinterDetailedInfoAsync(int id) =>
            _dbContext.Printers
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
    }
}