using AutoMapper.Configuration.Annotations;
using Microsoft.EntityFrameworkCore;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Persistence.Entities;

namespace PrintMe.Server.Persistence.Repository
{
    public sealed class OrderRepository
    {
        private readonly PrintMeDbContext _dbContext;

        public OrderRepository(PrintMeDbContext dbContext) => _dbContext = dbContext;

        public async Task<PrintOrder> CreateOrderAsync(PrintOrder order)
        {
            var added = await _dbContext.PrintOrders.AddAsync(order);
            await _dbContext.SaveChangesAsync();

            return added?.Entity;
        }

		public IAsyncEnumerable<PrintOrder> GetOrdersByUserId(int id) =>
			_dbContext.PrintOrders
				.Include(order => order.Printer)
				.Where(order => order.UserId == id)
				.AsAsyncEnumerable();
		
		public IAsyncEnumerable<PrintOrder> GetOrdersForPrinterOwnerAsync(int id) =>
			_dbContext.PrintOrders
				.Include(order => order.Printer)
				.Where(order => order.Printer.UserId == id)
				.AsAsyncEnumerable();

		public async Task<PrintOrder?> UpdateOrderAsync(int orderId, PrintOrder order)
		{
			var existingOrder = await _dbContext.PrintOrders
				.Include(o => o.Printer)
					.ThenInclude(p => p.User)
				.FirstOrDefaultAsync(o => o.PrintOrderId == orderId);

			if (existingOrder == null)
				return null;

			if (order.Price != default)
				existingOrder.Price = order.Price;
			if (order.OrderDate != default)
				existingOrder.OrderDate = order.OrderDate;
			if (order.DueDate != default)
				existingOrder.DueDate = order.DueDate;
			if (!string.IsNullOrEmpty(order.ItemLink))
				existingOrder.ItemLink = order.ItemLink;
			if (order.ItemQuantity.HasValue)
				existingOrder.ItemQuantity = order.ItemQuantity;
			if (!string.IsNullOrEmpty(order.ItemDescription))
				existingOrder.ItemDescription = order.ItemDescription;
			if (order.ItemMaterialId.HasValue)
				existingOrder.ItemMaterialId = order.ItemMaterialId;

			await _dbContext.SaveChangesAsync();
			
			await _dbContext.Entry(existingOrder)
				.Reference(o => o.Printer)
				.LoadAsync();
			
			if (existingOrder.Printer != null)
			{
				await _dbContext.Entry(existingOrder.Printer)
					.Reference(p => p.User)
					.LoadAsync();
			}
			
			return existingOrder;
		}

		public async Task<PrintOrder> RemoveOrderByIdAsync(int orderId)
		{
			var existing = await _dbContext.PrintOrders
				.AsQueryable()
				.FirstAsync(existing => existing.PrintOrderId == orderId);

			var removed = _dbContext.PrintOrders.Remove(existing);
			await _dbContext.SaveChangesAsync();

			return removed?.Entity;
		}

		public Task<PrintOrder> GetOrderById(int orderId) =>
			_dbContext.PrintOrders
				.Include(order => order.Printer)
				.Include(order => order.User)
				.Include(order => order.PrintOrderStatus)
				.Include(order => order.PrintOrderStatusReason)
				.Include(order => order.ItemMaterial)
				.AsQueryable()
				.FirstAsync(order => order.PrintOrderId == orderId);

	}
}