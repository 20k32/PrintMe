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
	/// <summary>
	/// Controller for managing print orders in the system.
	/// </summary>
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

		/// <summary>
		/// Creates a new print order.
		/// </summary>
		/// <param name="orderRequest">The order details from the client.</param>
		/// <returns>The created order details or error information.</returns>
		/// <response code="200">Returns the created order.</response>
		/// <response code="400">If the request is invalid or missing required data.</response>
		/// <response code="401">If user authentication fails.</response>
		/// <response code="403">If the order cannot be created due to business rules.</response>
		/// <response code="500">If there was an internal server error.</response>
		[HttpPost]
		[ProducesResponseType(typeof(ApiResult<PrintOrderDto>), 200)]
		public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest orderRequest)
		{
			PlainResult result;
			int userId;

			if (orderRequest is null)
			{
				result = new("Missing body.", StatusCodes.Status400BadRequest);
			}
			else if (orderRequest.IsNull())
			{
				result = new("Missing parameters in body.", StatusCodes.Status400BadRequest);
			}
			else
			{
				try
				{
					var userIdStr = Request.TryGetUserId();

					if (string.IsNullOrWhiteSpace(userIdStr))
					{
						result = new("Missing parameters in JWT.", StatusCodes.Status400BadRequest);
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

		/// <summary>
		/// Retrieves all orders for the currently authenticated user.
		/// </summary>
		/// <returns>A collection of orders belonging to the current user.</returns>
		/// <response code="200">Returns the list of orders.</response>
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

		/// <summary>
		/// Retrieves all orders for a specific user.
		/// </summary>
		/// <param name="userId">The ID of the user whose orders to retrieve.</param>
		/// <returns>A collection of orders for the specified user.</returns>
		/// <response code="200">Returns the list of orders.</response>
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

		/// <summary>
		/// Retrieves a specific order by its ID.
		/// </summary>
		/// <param name="orderId">The ID of the order to retrieve.</param>
		/// <returns>The requested order details or error information.</returns>
		/// <response code="200">Returns the requested order.</response>
		/// <response code="403">If the order cannot be accessed.</response>
		/// <response code="404">If the order is not found.</response>
		/// <response code="500">If there was an internal server error.</response>
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

		/// <summary>
		/// Deletes a specific order by its ID.
		/// </summary>
		/// <param name="orderId">The ID of the order to delete.</param>
		/// <returns>The deleted order details or error information.</returns>
		/// <response code="200">Returns the deleted order details.</response>
		/// <response code="400">If the order ID is invalid.</response>
		/// <response code="403">If the order cannot be deleted.</response>
		/// <response code="500">If there was an internal server error.</response>
		[HttpDelete("{orderId}")]
		public async Task<IActionResult> DeleteOrderById(int orderId)
		{
			PlainResult result;

			if (orderId < 1)
			{
				result = new("Missing body.", StatusCodes.Status400BadRequest);
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

		/// <summary>
		/// Updates all details of an existing order.
		/// </summary>
		/// <param name="orderDto">The updated order details.</param>
		/// <returns>The updated order information or error details.</returns>
		/// <response code="200">Returns the updated order.</response>
		/// <response code="400">If the request is invalid or missing required data.</response>
		/// <response code="403">If the order cannot be updated.</response>
		/// <response code="500">If there was an internal server error.</response>
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

		/// <summary>
		/// Updates some details of an existing order.
		/// </summary>
		/// <param name="orderDto"></param>
		/// <returns> The updated order information or error details.</returns>
		[HttpPut("PartialUpdate")]
		public async Task<IActionResult> PartialUpdateOrderById([FromBody] UpdatePartialOrderRequest orderDto)
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
				catch (InvalidOrderStatusException ex)
				{
					result = new($"{ex.Message}.{ex.InnerException?.Message ?? string.Empty}",
						StatusCodes.Status403Forbidden);
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

		/// <summary>
		/// Aborts pending order.
		/// </summary>
		/// <param name="orderId"></param>
		/// <returns> The updated order information or error details.</returns>
		[HttpPut("Abort")]
		public async Task<IActionResult> AbortOrderById([FromQuery] int orderId)
		{
			PlainResult result;


			try
			{
				var order = await _orderService.AbortOrderByIdAsync(orderId);
				result = new ApiResult<PrintOrderDto>(order, "Order aborted.",
					StatusCodes.Status200OK);
			}
			catch (InvalidOrderStatusException ex)
			{
				result = new($"{ex.Message}.{ex.InnerException?.Message ?? string.Empty}",
					StatusCodes.Status403Forbidden);
			}
			catch (NotFoundOrderInDbException ex)
			{
				result = new($"{ex.Message}.{ex.InnerException?.Message ?? string.Empty}",
					StatusCodes.Status403Forbidden);
			}
			catch (Exception ex)
			{
				result = new($"Internal server error while updating order.\n{ex.Message}\n{ex.StackTrace}",
					StatusCodes.Status500InternalServerError);
			}


			return result.ToObjectResult();
		}
	}
}