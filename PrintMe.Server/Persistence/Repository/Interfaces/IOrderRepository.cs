using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository.Interfaces;

public interface IOrderRepository
{
    Task<PrintOrder> CreateOrderAsync(PrintOrder order);
    IAsyncEnumerable<PrintOrder> GetOrdersByUserId(int id);
    IAsyncEnumerable<PrintOrder> GetOrdersForPrinterOwnerAsync(int id);
    Task<PrintOrder?> UpdateOrderAsync(int orderId, PrintOrder order);
    Task<PrintOrder> RemoveOrderByIdAsync(int orderId);
    Task<PrintOrder> GetOrderById(int orderId);
}