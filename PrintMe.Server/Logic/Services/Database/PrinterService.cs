using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;
using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Logic.Services.Database
{
    internal sealed class PrinterService(IPrinterRepository repository, IMapper mapper) : IPrinterService
    {
        private readonly IMapper _mapper = mapper;
        private readonly IPrinterRepository _repository = repository;
        
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
        
        public async Task<IEnumerable<PrinterDto>> GetPrintersDetailedByUserId(int id, bool? isDeactivated = null)
        {
            ICollection<Printer> printers = await _repository.GetPrintersForUserAsync(id, isDeactivated);

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

        private async Task GetPrinterModelByName(string name)
        {
            var model = await _repository.GetModelByNameAsync(name);
        }

        public async Task AddPrinterAsync(PrinterDto printerDto)
        {
            var printer = _mapper.Map<Printer>(printerDto);

            printer.PrinterModel = await _repository.GetModelByNameAsync(printer.PrinterModel.Name);

            printer.Materials ??= [];
            foreach (var item in printerDto.Materials)
            {
                var materialRaw = await repository.GetMaterialByIdAsync(item.PrintMaterialId);
                printer.Materials.Add(materialRaw);
            }
            
            await _repository.AddPrinterAsync(printer);
        }
        
        public async IAsyncEnumerable<PrinterLocationDto> GetPrinterLocationAsync(ICollection<PrintMaterialDto> material, double? maxHeight, double? maxWidth)
        {
            await foreach (var printer in _repository.GetPrinterLocationAsync(material, maxHeight, maxWidth))
            {
                yield return printer;
            }
        }

        public async Task<List<PrintMaterialDto>> GetMaterialsAsync()
        {
            var materials = await _repository.GetAllMaterialsAsync();
            if (materials is null || materials.Count == 0)
            {
                throw new NotFoundMaterialInDbException();
            }
            return materials.Select(material => material.MapToDto()).ToList();
        }

        public async Task<List<PrinterModelDto>> GetModelsAsync()
        {
            var models = await _repository.GetAllModelsAsync();
            if (models is null || models.Count == 0)
            {
                throw new NotFoundPrinterModelInDbException();
            }
            return models.Select(model => model.MapToDto()).ToList();
        }

        public async Task DeactivatePrinterAsync(int printerId)
        {
            await _repository.DeactivatePrinterAsync(printerId);
        }

        public async Task ActivatePrinterAsync(int printerId)
        {
            await _repository.ActivatePrinterAsync(printerId);
        }
    }
}