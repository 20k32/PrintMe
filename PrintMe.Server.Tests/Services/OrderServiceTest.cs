using AutoMapper;
using Moq;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;
using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Tests.Services;

public class OrderServiceTest
{
    [Fact]
    public async Task AddOrderAsync_ShouldAddOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);

        var userId = 1;
        var createOrderRequest = new CreateOrderRequest();
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();

        mockMapper.Setup(m => m.Map<CreateOrderRequest, PrintOrder>(It.IsAny<CreateOrderRequest>(), It.IsAny<Action<IMappingOperationOptions>>()))
            .Returns(printOrder);
        mockOrderRepository.Setup(r => r.CreateOrderAsync(It.IsAny<PrintOrder>()))
            .ReturnsAsync(printOrder);
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await orderService.AddOrderAsync(userId, createOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }
}