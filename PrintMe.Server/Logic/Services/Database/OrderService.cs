using AutoMapper;
using PrintMe.Server.Constants;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;

namespace PrintMe.Server.Logic.Services.Database
{
    internal sealed class OrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly UserService _userService;
        private readonly IMapper _mapper;


        public OrderService(IMapper mapper, UserService userService, OrderRepository orderRepository)
            => (_mapper, _userService, _orderRepository) = (mapper, userService, orderRepository);

        public async IAsyncEnumerable<PrintOrderDto> GetOrdersByUserIdAsync(int userId)
        {
            _ = await _userService.GetUserByIdAsync(userId);

            await foreach (var orderRaw in _orderRepository.GetOrdersByUserId(userId))
            {
                var orderDto = _mapper.Map<PrintOrderDto>(orderRaw);
                orderDto.ExecutorId = orderRaw.Printer.UserId;
                yield return orderDto;
            }
        }
        
        public async IAsyncEnumerable<PrintOrderDto> GetOrdersForPrinterOwnerAsync(int userId)
        {
            _ = await _userService.GetUserByIdAsync(userId);

            await foreach (var orderRaw in _orderRepository.GetOrdersForPrinterOwnerAsync(userId))
            {
                var orderDto = _mapper.Map<PrintOrderDto>(orderRaw);
                // orderDto.ExecutorId = orderRaw.Printer.UserId;
                yield return orderDto;
            }
        }
        
        public async Task<PrintOrderDto> UpdateOrderByIdAsync(int orderId, UpdateFullOrderRequest request)
        {
            var orderRaw = _mapper.Map<PrintOrder>(request);

            var orderResult = await _orderRepository.UpdateOrderAsync(orderId, orderRaw);

            if (orderResult is null)
            {
                throw new NotFoundOrderInDbException();
            }

            return _mapper.Map<PrintOrderDto>(orderResult);
        }

        public async Task<PrintOrderDto> RemoveOrderByIdAsync(int orderId)
        {
            try
            {
                var orderRaw = await _orderRepository.RemoveOrderByIdAsync(orderId);

                if (orderRaw is null)
                {
                    throw new NotFoundOrderInDbException();
                }
                
                return _mapper.Map<PrintOrderDto>(orderRaw);
            }
            catch (Exception ex)
            {
                throw new NotFoundOrderInDbException(ex);
            }
        }

        public async Task<PrintOrderDto> AddOrderAsync(int userId, CreateOrderRequest request)
        {
            var orderRaw = _mapper.Map<CreateOrderRequest, PrintOrder>(request,
                options => options.AfterMap((_, d) =>
                {
                    d.UserId = userId;
                    d.PrintOrderStatusId = DbConstants.PrintOrderStatus.Dictionary[DbConstants.PrintOrderStatus.Pending];
                }));

            var result = await _orderRepository.CreateOrderAsync(orderRaw);

            return _mapper.Map<PrintOrderDto>(result);
        }

        public async Task<PrintOrderDto> GetOrderByIdAsync(int orderId)
        {
            var result = await _orderRepository.GetOrderById(orderId);

            if (result is null)
            {
                throw new NotFoundOrderInDbException();
            }

            var orderDto = _mapper.Map<PrintOrderDto>(result);
            orderDto.ExecutorId = result.Printer.UserId;

            return orderDto;
        }
    }
}