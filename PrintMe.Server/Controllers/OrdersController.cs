using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using PrintMe.Server.Logic;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Models.Api;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.DTOs.UserDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Models.Extensions;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(IServiceProvider provider)
        {
            _orderService = provider.GetService<OrderService>();
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ApiResult<PrintOrderDto>), 200)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
        {
            PlainResult result;
            int userId;
            
            if (orderRequest is null)
            {
                result = new ( "Missing body.", StatusCodes.Status400BadRequest);
            }
            else if (orderRequest.IsNull())
            {
                result = new ("Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var userIdStr = Request.TryGetUserId();
                    
                    if (string.IsNullOrWhiteSpace(userIdStr))
                    {
                        result = new ("Missing parameters in JWT.", StatusCodes.Status400BadRequest);
                    }
                    else if (!int.TryParse(userIdStr, out userId))
                    {
                        result = new("Incorrect id.",
                            StatusCodes.Status401Unauthorized);
                    }
                    else
                    {
                        var order = await _orderService.AddOrderAsync(userId, orderRequest);
                        result = new ApiResult<PrintOrderDto>(order, "Order added.",
                            StatusCodes.Status200OK);
                    }
                }
                catch (NotFoundOrderInDbException ex)
                {
                    result = new(ex.Message, StatusCodes.Status403Forbidden);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while creating order.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return result.ToObjectResult();
        }
        
        [HttpGet("my")]
        [ProducesResponseType(typeof(PrintOrderDto), 200)]
        public IAsyncEnumerable<PrintOrderDto> GetMyOrders()
        {
            IAsyncEnumerable<PrintOrderDto> result;
            
            try
            {
                var userIdStr = Request.TryGetUserId();
                var userId = int.Parse(userIdStr);
                
                result = _orderService.GetOrdersByUserIdAsync(userId);
            }
            catch
            {
                result = Enumerable.Empty<PrintOrderDto>().ToAsyncEnumerable();
            }

            return result;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(PrintOrderDto), 200)]
        public IAsyncEnumerable<PrintOrderDto> GetOrdersByUserId([FromQuery] int userId)
        {
            IAsyncEnumerable<PrintOrderDto> result;

            if (userId < 1)
            {
                result = Enumerable.Empty<PrintOrderDto>().ToAsyncEnumerable();
            }
            else
            {
                result = _orderService.GetOrdersByUserIdAsync(userId);
            }

            return result;
        }
        
        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(PrintOrderDto), 200)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            PlainResult result;

            if (orderId < 1)
            {
                result = new("Id is incorrect", StatusCodes.Status404NotFound);
            }
            else
            {
                try
                {
                    var order = await _orderService.GetOrderByIdAsync(orderId);
                    
                    result = new ApiResult<PrintOrderDto>(order, "Such order exists in database.",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundOrderInDbException ex)
                {
                    result = new($"{ex.Message}",
                        StatusCodes.Status403Forbidden);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while deleting order.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }

            return result.ToObjectResult();
        }
        
        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrderById(int orderId)
        {
            PlainResult result;

            if (orderId < 1)
            {
                result = new ( "Missing body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var order = await _orderService.RemoveOrderByIdAsync(orderId);
                    result = new ApiResult<PrintOrderDto>(order, "Order deleted from database.",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundOrderInDbException ex)
                {
                    result = new($"{ex.Message}.{ex?.InnerException?.Message ?? string.Empty}",
                        StatusCodes.Status403Forbidden);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while deleting order.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return result.ToObjectResult();
        }
        
        [HttpPut("FullUpdate")]
        public async Task<IActionResult> FullUpdateOrderById([FromBody] UpdateFullOrderRequest orderDto)
        {
            PlainResult result;

            if (orderDto is null)
            {
                result = new("Missing body.", StatusCodes.Status400BadRequest);
            }
            else if (orderDto.IsNull())
            {
                result = new("Missing parameters in body.", StatusCodes.Status400BadRequest);
            }
            else
            {
                try
                {
                    var order = await _orderService.UpdateOrderByIdAsync(orderDto.OrderId, orderDto);
                    result = new ApiResult<PrintOrderDto>(order, "Order updated.",
                        StatusCodes.Status200OK);
                }
                catch (NotFoundOrderInDbException ex)
                {
                    result = new($"{ex.Message}.{ex?.InnerException?.Message ?? string.Empty}",
                        StatusCodes.Status403Forbidden);
                }
                catch (Exception ex)
                {
                    result = new($"Internal server error while updating order.\n{ex.Message}\n{ex.StackTrace}",
                        StatusCodes.Status500InternalServerError);
                }
            }
            
            return result.ToObjectResult();
        }
    }
}