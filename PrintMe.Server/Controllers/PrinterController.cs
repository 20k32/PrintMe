using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using PrintMe.Server.Logic;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs.PrinterDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Controllers
{
    [ApiController]
    [Route("api/Printers")]
    public class PrinterController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _provider;

        public PrinterController(IServiceProvider provider, IConfiguration configuration)
        {
            _provider = provider;
            _configuration = configuration;
        }

        private bool CheckIfRangeCorrect(int skip, int take)
        {
            var maxEntriesCount = _configuration.GetMaxPrintersInResponse();
            var absoluteSkip = Math.Abs(skip);
            var absoluteTake = Math.Abs(take);
            var absoluteDifference = Math.Abs(absoluteSkip - absoluteTake);

            return absoluteDifference <= maxEntriesCount;
        }

        /// <summary>
        /// <para>Start streaming printers.</para>
        /// accepts 'detailed' parameter from query string
        /// </summary>
        [HttpGet("all")]
        [ProducesResponseType(typeof(List<SimplePrinterDto>), 200)]
        public IAsyncEnumerable<SimplePrinterDto> GetPrinterInfo([FromQuery] int skip, [FromQuery] int take,
            [FromQuery] bool detailed = false)
        {
            IAsyncEnumerable<SimplePrinterDto> result;

            var isRangeCorrect = CheckIfRangeCorrect(skip, take);

            if (!isRangeCorrect)
            {
                result = Enumerable.Empty<SimplePrinterDto>().ToAsyncEnumerable();
            }
            else
            {
                var printerService = _provider.GetService<PrinterService>();

                if (detailed)
                {
                    result = printerService.GetPrintersDetailedAsync(skip, take);
                }
                else
                {
                    result = printerService.GetPrintersBasicAsync(skip, take);
                }
            }

            return result;
        }

        /// <summary>
        /// Get info for printer by its id
        /// accepts 'detailed' parameter from query string
        /// </summary>
        [HttpGet("/api/printers/{id:int}")]
        [ProducesResponseType(typeof(ApiResult<SimplePrinterDto>), 200)]
        public async Task<IActionResult> GetBasicInfoById(int? id, [FromQuery] bool detailed = false)
        {
            PlainResult result;

            if (id is null)
            {
                result = new("Missing id.",
                    StatusCodes.Status401Unauthorized);
            }
            else
            {
                var printerService = _provider.GetService<PrinterService>();
                try
                {
                    if (detailed)
                    {
                        var printer = await printerService.GetPrinterDetailedByIdAsync(id.Value);

                        result = new ApiResult<PrinterDto>(printer, "There is such printer in database",
                            StatusCodes.Status200OK);
                    }
                    else
                    {
                        var printer = await printerService.GetPrinterBasicByIdAsync(id.Value);

                        result = new ApiResult<SimplePrinterDto>(printer, "There is such printer in database",
                            StatusCodes.Status200OK);
                    }

                }
                catch (NotFoundPrinterInDbException ex)
                {
                    result = new(ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }

            return result.ToObjectResult();
        }

        /// <summary>
        /// Get info for printer by user_id
        /// accepts 'detailed' parameter from query string
        /// </summary>
        [HttpGet("/api/printers/user/{userId:int}")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<PrinterDto>>), 200)]
        public async Task<IActionResult> GetInfoByUserId(int? userId, [FromQuery] bool detailed = false)
        {
            PlainResult result;

            if (userId is null)
            {
                result = new("Missing id.",
                    StatusCodes.Status401Unauthorized);
            }
            else
            {
                var printerService = _provider.GetService<PrinterService>();
                try
                {
                    if (detailed)
                    {
                        var printer = await printerService.GetPrintersDetailedByUserId(userId.Value);

                        if (printer is not null && printer.Count() != 0) // no multiple enumeration
                        {
                            result = new ApiResult<IEnumerable<PrinterDto>>(printer,
                                "There is some printers in database",
                                StatusCodes.Status200OK);
                        }
                        else
                        {
                            result = new("There is no printers related to such user",
                                StatusCodes.Status404NotFound);
                        }
                    }
                    else
                    {
                        var printer = await printerService.GetPrintersBasicByUserId(userId.Value);

                        if (printer is not null && printer.Count() != 0) // no multiple enumeration
                        {
                            result = new ApiResult<IEnumerable<SimplePrinterDto>>(printer,
                                "There is some printers in database",
                                StatusCodes.Status200OK);
                        }
                        else
                        {
                            result = new("There is no printers related to such user",
                                StatusCodes.Status404NotFound);
                        }
                    }
                }
                catch (NotFoundPrinterInDbException ex)
                {
                    result = new(ex.Message,
                        StatusCodes.Status404NotFound);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }

            return result.ToObjectResult();
        }

        /// <summary>
        /// Get info for printers by user jwt
        /// accepts 'isDeactivated' parameter from query string
        /// </summary>
        [HttpGet("/api/printers/my")]
        [ProducesResponseType(typeof(ApiResult<IEnumerable<PrinterDto>>), 200)]
        [Authorize]
        public async Task<IActionResult> GetMyPrinterDetailedInfo([FromQuery] bool? isDeactivated)
        {
            PlainResult result;

            var printerService = _provider.GetService<PrinterService>();
            try
            {
                var idString = Request.TryGetUserId();

                if (idString is null)
                {
                    result = new("Missing id.",
                        StatusCodes.Status401Unauthorized);
                }
                else if (int.TryParse(idString, out var id))
                {
                    // if (detailed)
                    // {
                        var printer = await printerService.GetPrintersDetailedByUserId(id, isDeactivated);

                        if (printer is not null && printer.Count() != 0)
                        {
                            result = new ApiResult<IEnumerable<PrinterDto>>(printer,
                                "There is some printers in database",
                                StatusCodes.Status200OK);
                        }
                        else
                        {
                            result = new("There is no printers related to such user",
                                StatusCodes.Status404NotFound);
                        }
                    // }
                    // else
                    // {
                    //     var printer = await printerService.GetPrintersBasicByUserId(id);
                    //
                    //     if (printer is not null && printer.Count() != 0) // no multiple enumeration
                    //     {
                    //         result = new ApiResult<IEnumerable<SimplePrinterDto>>(printer,
                    //             "There is some printers in database",
                    //             StatusCodes.Status200OK);
                    //     }
                    //     else
                    //     {
                    //         result = new("There is no printers related to such user",
                    //             StatusCodes.Status404NotFound);
                    //     }
                    // }

                }
                else
                {
                    result = new("Incorrect id",
                        StatusCodes.Status401Unauthorized);
                }
            }
            catch (NotFoundPrinterInDbException ex)
            {
                result = new(ex.Message,
                    StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                result = new($"Internal server error while getting user\n{ex.Message}\n{ex.StackTrace}",
                    StatusCodes.Status500InternalServerError);
            }

            return result.ToObjectResult();
        }

        /// <summary>
        /// <para>Start streaming printers locations.</para>
        /// accepts 'materials', 'maxModelHeight' and 'maxModelWidth' parameters from query
        /// </summary>
        [HttpPut("markers")]
        [ProducesResponseType(typeof(List<PrinterLocationDto>), 200)]
        public IAsyncEnumerable<PrinterLocationDto> GetPrinterLocations([FromBody] GetPrinterLocationRequest request)
        {
            IAsyncEnumerable<PrinterLocationDto> result;

            var materials = request.Materials;
            var maxHeight = request.MaxModelHeight;
            var maxWidth = request.MaxModelWidth;

            var printerService = _provider.GetService<PrinterService>();

            result = printerService.GetPrinterLocationAsync(materials, maxHeight, maxWidth);

            return result;
        }

        /// <summary>
        /// Get all materials
        /// </summary>
        [HttpGet("materials")]
        [ProducesResponseType(typeof(List<PrintMaterialDto>), 200)]
        public async Task<IActionResult> GetMaterials()
        {
            var printerService = _provider.GetService<PrinterService>();
            try
            {
                var materials = await printerService.GetMaterialsAsync();
                return Ok(materials);
            }
            catch (NotFoundMaterialInDbException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get all models
        /// </summary>
        [HttpGet("models")]
        [ProducesResponseType(typeof(List<PrinterModelDto>), 200)]
        public async Task<IActionResult> GetModels()
        {
            var printerService = _provider.GetService<PrinterService>();
            try
            {
                var models = await printerService.GetModelsAsync();
                return Ok(models);
            }
            catch (NotFoundPrinterModelInDbException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Deactivates printer by id
        /// </summary>
        /// <param name="printerId"></param>
        /// <returns></returns>
        [HttpPost("deactivate/{printerId:int}")]
        [Authorize]
        public async Task<IActionResult> DeactivatePrinter(int printerId)
        {
            var printerService = _provider.GetService<PrinterService>();
            var userId = Request.TryGetUserId();
            var userPrinters = await printerService.GetPrintersDetailedByUserId(int.Parse(userId));
            if (userPrinters.All(dto => dto.Id != printerId))
            {
                return BadRequest(new { message = "User does not have such a printer" });
            }
            try
            {
                await printerService.DeactivatePrinterAsync(printerId);
                return Ok(new { message = "Printer deactivated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error deactivating printer: {ex.Message}" });
            }
        }
    }
}