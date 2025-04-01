using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;

namespace PrintMe.Server.Logic.Services.Database.Interfaces;

public interface IOrderService
{
    IAsyncEnumerable<PrintOrderDto> GetOrdersByUserIdAsync(int userId);
    IAsyncEnumerable<PrintOrderDto> GetOrdersForPrinterOwnerAsync(int userId);
    Task<PrintOrderDto> UpdateOrderByIdAsync(int orderId, UpdateFullOrderRequest request);
    Task<PrintOrderDto> UpdateOrderByIdAsync(int orderId, UpdatePartialOrderRequest request);
    Task<PrintOrderDto> RemoveOrderByIdAsync(int orderId);
    Task<PrintOrderDto> AddOrderAsync(int userId, CreateOrderRequest request);
    Task<PrintOrderDto> GetOrderByIdAsync(int orderId);
    Task<PrintOrderDto> AbortOrderByIdAsync(int orderId);
}