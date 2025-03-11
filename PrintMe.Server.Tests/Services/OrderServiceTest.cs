using AutoMapper;
using Moq;
using PrintMe.Server.Logic.Services.Database;
using PrintMe.Server.Logic.Services.Database.Interfaces;
using PrintMe.Server.Models.Api.ApiRequest;
using PrintMe.Server.Models.DTOs;
using PrintMe.Server.Models.DTOs.UserDto;
using PrintMe.Server.Models.Exceptions;
using PrintMe.Server.Persistence.Entities;
using PrintMe.Server.Persistence.Repository;
using PrintMe.Server.Persistence.Repository.Interfaces;

namespace PrintMe.Server.Tests.Services;

public class OrderServiceTest
{
    [Fact]
    public async Task GetOrdersByUserIdAsync_ShouldReturnOrders()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var mockUser = new PasswordUserDto { UserId = 1 };
        var printOrder = new PrintOrder { Printer = new Printer { UserId = 2 } };
        var printOrderDto = new PrintOrderDto();

        mockUserService.Setup(u => u.GetUserByIdAsync(mockUser.UserId))
            .ReturnsAsync(mockUser);
        mockOrderRepository.Setup(r => r.GetOrdersByUserId(mockUser.UserId))
            .Returns(new List<PrintOrder> { printOrder }.ToAsyncEnumerable());
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await orderService.GetOrdersByUserIdAsync(mockUser.UserId).ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(printOrderDto, result.First());
        Assert.Equal(printOrder.Printer.UserId, result.First().ExecutorId);
    }
    
    [Fact]
    public async Task GetOrdersForPrinterOwnerAsync_ShouldReturnOrders()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var mockUser = new PasswordUserDto { UserId = 1 };
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();

        mockUserService.Setup(u => u.GetUserByIdAsync(mockUser.UserId))
            .ReturnsAsync(mockUser);
        mockOrderRepository.Setup(r => r.GetOrdersForPrinterOwnerAsync(mockUser.UserId))
            .Returns(new List<PrintOrder> { printOrder }.ToAsyncEnumerable());
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await orderService.GetOrdersForPrinterOwnerAsync(mockUser.UserId).ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(printOrderDto, result.First());
    }
    
    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldFullUpdateOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var updateFullOrderRequest = new UpdateFullOrderRequest()
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1, Price = 1, ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1 };;
        var printOrderDto = new PrintOrderDto();
        
        mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdateFullOrderRequest>()))
            .Returns(printOrder);
        mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await orderService.UpdateOrderByIdAsync(orderId, updateFullOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldThrowNotFoundOrderInDbExceptionWhenFullUpdate()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();
        
        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var updateFullOrderRequest = new UpdateFullOrderRequest()
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1, Price = 1, ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1 };;
        var printOrderDto = new PrintOrderDto();
        
        mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdateFullOrderRequest>()))
            .Returns(printOrder);
        mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => orderService.UpdateOrderByIdAsync(orderId, updateFullOrderRequest));
    }
    
    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldPartialUpdateOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var updatePartialOrderRequest = new UpdatePartialOrderRequest
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();
        
        mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdatePartialOrderRequest>()))
            .Returns(printOrder);
        mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await orderService.UpdateOrderByIdAsync(orderId, updatePartialOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldThrowInvalidOrderStatusExceptionWhenPartialUpdate()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var updatePartialOrderRequest = new UpdatePartialOrderRequest
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();
        
        mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdatePartialOrderRequest>()))
            .Returns(printOrder);
        mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
        
        // Act and Assert
        await Assert.ThrowsAsync<InvalidOrderStatusException>(() => orderService.UpdateOrderByIdAsync(orderId, updatePartialOrderRequest));
    }
    
    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldThrowNotFoundOrderInDbExceptionWhenPartialUpdate()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var updatePartialOrderRequest = new UpdatePartialOrderRequest
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();
        
        mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdatePartialOrderRequest>()))
            .Returns(printOrder);
        mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => orderService.UpdateOrderByIdAsync(orderId, updatePartialOrderRequest));
    }
    
    [Fact]
    public async Task RemoveOrderByIdAsync_ShouldRemoveOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();

        mockOrderRepository.Setup(r => r.RemoveOrderByIdAsync(orderId))
            .ReturnsAsync(printOrder);
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await orderService.RemoveOrderByIdAsync(orderId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }
    
    [Fact]
    public async Task AddOrderAsync_ShouldAddOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);

        var userId = 0;
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
    
    [Fact]
    public async Task RemoveOrderByIdAsync_ShouldThrowNotFoundOrderInDbException()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();

        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;

        mockOrderRepository.Setup(r => r.RemoveOrderByIdAsync(orderId))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => orderService.RemoveOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();
    
        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
    
        var orderId = 1;
        var printOrder = new PrintOrder()
        {
            PrintOrderId = orderId,
            Printer = new Printer(),
            PrinterId = 1
        };
        var printOrderDto = new PrintOrderDto();
    
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
        mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync(printOrder);
    
        // Act
        var result = await orderService.GetOrderByIdAsync(orderId);
    
        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldThrowNotFoundOrderInDbException()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();
    
        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
    
        var orderId = 1;
        var printOrderDto = new PrintOrderDto();
    
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
        mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => orderService.GetOrderByIdAsync(orderId));
    }
    
    [Fact]
    public async Task AbortOrderById_ShouldAbortOrder()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();
    
        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
    
        var orderId = 1;
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();
    
        mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync(printOrder);
        mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
    
        // Act
        var result = await orderService.AbortOrderByIdAsync(orderId);
    
        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task AbortOrderById_ShouldThrowNotFoundInDbException()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();
    
        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => orderService.AbortOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task AbortOrderById_ShouldThrowInvalidOrderStatusException()
    {
        // Arrange
        var mockMapper = new Mock<IMapper>();
        var mockUserService = new Mock<IUserService>();
        var mockOrderRepository = new Mock<IOrderRepository>();
    
        var orderService = new OrderService(mockMapper.Object, mockUserService.Object, mockOrderRepository.Object);
        
        var orderId = 1;
        var printOrder = new PrintOrder{ PrintOrderStatusId = 2 };
        
        mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync(printOrder);
        
        // Act and Assert
        await Assert.ThrowsAsync<InvalidOrderStatusException>(() => orderService.AbortOrderByIdAsync(orderId));
    }
}