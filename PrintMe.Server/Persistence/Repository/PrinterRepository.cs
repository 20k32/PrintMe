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
                               .Where(printer => !printer.IsDeactivated)
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
                               .Where(printer => !printer.IsDeactivated)
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

        public Task<List<Printer>> GetPrintersForUserAsync(int userId, bool? isDeactivated)
        {
            var query = _dbContext.Printers
                .AsNoTracking()
                .Include(printer => printer.PrinterModel)
                .Include(printer => printer.Materials)
                .Where(printer => printer.UserId == userId);

            if (isDeactivated.HasValue)
            {
                query = query.Where(printer => printer.IsDeactivated == isDeactivated.Value);
            }

            return query.ToListAsync();
        }

        public async Task DeactivatePrinterAsync(int printerId)
        {
            var printer = await _dbContext.Printers.FindAsync(printerId);
            if (printer != null)
            {
                printer.IsDeactivated = true;
                await _dbContext.SaveChangesAsync();
            }
        }

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
        
        public async IAsyncEnumerable<PrinterLocationDto> GetPrinterLocationAsync(ICollection<PrintMaterialDto> material, double? maxHeight, double? maxWidth)
        {
            var query = _dbContext.Printers.AsNoTracking();

            if (material != null && material.Count != 0)
            {
                query = query.Where(printer => printer.Materials.Any(m => material.Select(mat => mat.PrintMaterialId).Contains(m.PrintMaterialId)));
            }

            if (maxHeight.HasValue)
            {
                query = query.Where(printer => printer.MaxModelHeight >= maxHeight.Value);
            }

            if (maxWidth.HasValue)
            {
                query = query.Where(printer => printer.MaxModelWidth >= maxWidth.Value);
            }

            await foreach (var printerRaw in query
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

        public Task<List<PrintMaterial>> GetAllMaterialsAsync() =>
            _dbContext.PrintMaterials1.AsNoTracking().ToListAsync();

        public Task<List<PrinterModel>> GetAllModelsAsync() =>
            _dbContext.PrinterModels.AsNoTracking().ToListAsync();

        public Task<PrinterModel> GetModelByNameAsync(string name) => _dbContext
            .PrinterModels
            .AsQueryable()
            .FirstAsync(model =>
            model.Name == name);
        
        public Task<PrinterModel> GetModelByIdAsync(int id) => _dbContext
            .PrinterModels
            .AsQueryable()
            .FirstAsync(model =>
                model.PrinterModelId == id);

        public Task<PrintMaterial> GetMaterialByIdAsync(int printMaterialId) =>
            _dbContext.PrintMaterials1.AsQueryable()
                .FirstAsync(material => material.PrintMaterialId == printMaterialId);
    }
}