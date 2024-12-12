using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database
{
    internal sealed class PrinterService
    {
        private PrinterRepository _repository;
        public PrinterService(PrinterRepository repository) => _repository = repository;
        
        public async IAsyncEnumerable<SimplePrinterDto> GetPrintersBasicAsync(int skip, int take)
        { 
            await foreach (var simplePrinter in _repository.GetSimplePrinterStreamAsync(skip , take))
            {
                var printerDto = simplePrinter.MapToDto();
                yield return printerDto;
            }
        }

        public async IAsyncEnumerable<PrinterDto> GetPrintersDetailedAsync(int skip, int take)
        {
            
            await foreach (var simplePrinter in _repository.GetPrinterStreamAsync(skip, take))
            {
                var printerDto = simplePrinter.MapToDto();
                yield return printerDto;
            }
        }

        public async Task<SimplePrinterDto> GetPrinterBasicByIdAsync(int id)
        {
            var printer = await _repository.GetPrinterBasicInfoByIdAsync(id);

            if (printer is null)
            {
                throw new NotFoundPrinterInDbException();
            }
            
            var printerDto = printer.MapToDto();

            return printerDto;
        }

        public async Task<PrinterDto> GetPrinterDetailedByIdAsync(int id)
        {
            var printer = await _repository.GetPrinterDetailedInfoAsync(id);

            if (printer is null)
            {
                throw new NotFoundPrinterInDbException();
            }
            
            var printerDto = printer.MapToDto();

            return printerDto;
        }
        
        public async Task<IEnumerable<PrinterDto>> GetPrintersDetailedByUserId(int id)
        {
            ICollection<Printer> printers = await _repository.GetPrintersForUserAsync(id);

            if (printers is null)
            {
                throw new NotFoundPrinterInDbException();
            }

            var printersDto = printers.MapToDtos();

            return printersDto;
        }

        public async Task<IEnumerable<SimplePrinterDto>> GetPrintersBasicByUserId(int id)
        {
            ICollection<SimplePrinter> printers = await _repository.GetPrintersBasicInfoForUserAsync(id);

            if (printers is null)
            {
                throw new NotFoundPrinterInDbException();
            }

            var printersDto = printers.MapToDtos();

            return printersDto;
        }

        public async Task AddPrinterAsync(PrinterDto printerDto)
        {
            var printer = printerDto.MapToEntity();
            await _repository.AddPrinterAsync(printer);
        }
        
        public async IAsyncEnumerable<PrinterLocationDto> GetPrinterLocationAsync(ICollection<PrintMaterialDto> material, double? maxHeight, double? maxWidth)
        {
            await foreach (var printer in _repository.GetPrinterLocationAsync(material, maxHeight, maxWidth))
            {
                yield return printer;
            }
        }
    }
}