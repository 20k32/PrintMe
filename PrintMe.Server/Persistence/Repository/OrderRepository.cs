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
		
		public async Task<PrintOrder> UpdateOrderAsync(int orderId, PrintOrder order)
		{
			var existing = await _dbContext.PrintOrders
				.AsQueryable()
				.FirstAsync(existing => existing.PrintOrderId == orderId);

			existing.Price = order.Price;

			existing.Printer = order.Printer;
			existing.PrinterId = order.PrinterId;

			existing.User = order.User;
			existing.UserId = order.UserId;

			existing.DueDate = order.DueDate;
			existing.OrderDate = order.OrderDate;
			existing.ItemDescription = order.ItemDescription;
			existing.ItemLink = order.ItemLink;
			existing.ItemQuantity = order.ItemQuantity;

			existing.PrintOrderStatus = order.PrintOrderStatus;
			existing.PrintOrderStatusId = order.PrintOrderStatusId;

			existing.PrintOrderStatusReason = order.PrintOrderStatusReason;
			existing.PrintOrderStatusReasonId = order.PrintOrderStatusReasonId;

			var updated = _dbContext.PrintOrders.Update(existing);
			await _dbContext.SaveChangesAsync();

			return updated?.Entity;
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