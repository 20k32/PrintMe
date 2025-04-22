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
    
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly OrderService _orderService;
    
    public OrderServiceTest()
    {
        _mockMapper = new Mock<IMapper>();
        _mockUserService = new Mock<IUserService>();
        _mockOrderRepository = new Mock<IOrderRepository>();
        _orderService = new OrderService(_mockMapper.Object, _mockUserService.Object, _mockOrderRepository.Object);
    }
    
    [Fact]
    public async Task GetOrdersByUserIdAsync_ShouldReturnOrders()
    {
        // Arrange
        var mockUser = new PasswordUserDto { UserId = 1 };
        var printOrder = new PrintOrder { Printer = new Printer { UserId = 2 } };
        var printOrderDto = new PrintOrderDto();

        _mockUserService.Setup(u => u.GetUserByIdAsync(mockUser.UserId))
            .ReturnsAsync(mockUser);
        _mockOrderRepository.Setup(r => r.GetOrdersByUserId(mockUser.UserId))
            .Returns(new List<PrintOrder> { printOrder }.ToAsyncEnumerable());
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.GetOrdersByUserIdAsync(mockUser.UserId).ToListAsync();

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
        var mockUser = new PasswordUserDto { UserId = 1 };
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();

        _mockUserService.Setup(u => u.GetUserByIdAsync(mockUser.UserId))
            .ReturnsAsync(mockUser);
        _mockOrderRepository.Setup(r => r.GetOrdersForPrinterOwnerAsync(mockUser.UserId))
            .Returns(new List<PrintOrder> { printOrder }.ToAsyncEnumerable());
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.GetOrdersForPrinterOwnerAsync(mockUser.UserId).ToListAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(printOrderDto, result.First());
    }
    
    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldFullUpdateOrder()
    {
        // Arrange
        var orderId = 1;
        var updateFullOrderRequest = new UpdateFullOrderRequest()
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1, Price = 1, ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1 };;
        var printOrderDto = new PrintOrderDto();
        
        _mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdateFullOrderRequest>()))
            .Returns(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.UpdateOrderByIdAsync(orderId, updateFullOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldThrowNotFoundOrderInDbExceptionWhenFullUpdate()
    {
        // Arrange
        var orderId = 1;
        var updateFullOrderRequest = new UpdateFullOrderRequest()
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1, Price = 1, ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1 };;
        var printOrderDto = new PrintOrderDto();
        
        _mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdateFullOrderRequest>()))
            .Returns(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.UpdateOrderByIdAsync(orderId, updateFullOrderRequest));
    }
    
    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldPartialUpdateOrder()
    {
        // Arrange
        var orderId = 1;
        var updatePartialOrderRequest = new UpdatePartialOrderRequest
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();
        
        _mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdatePartialOrderRequest>()))
            .Returns(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.UpdateOrderByIdAsync(orderId, updatePartialOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldThrowInvalidOrderStatusExceptionWhenPartialUpdate()
    {
        // Arrange
        var orderId = 1;
        var updatePartialOrderRequest = new UpdatePartialOrderRequest
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();
        
        _mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdatePartialOrderRequest>()))
            .Returns(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
        
        // Act and Assert
        await Assert.ThrowsAsync<InvalidOrderStatusException>(() => _orderService.UpdateOrderByIdAsync(orderId, updatePartialOrderRequest));
    }
    
    [Fact]
    public async Task UpdateOrderByIdAsync_ShouldThrowNotFoundOrderInDbExceptionWhenPartialUpdate()
    {
        // Arrange
        var orderId = 1;
        var updatePartialOrderRequest = new UpdatePartialOrderRequest
        {
            OrderId = 1, Price = 1, DueDate = "2022-01-01", ItemLink = "link", ItemQuantity = 1,
            ItemDescription = "desc", ItemMaterialId = 1
        };
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();
        
        _mockMapper.Setup(m => m.Map<PrintOrder>(It.IsAny<UpdatePartialOrderRequest>()))
            .Returns(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.UpdateOrderByIdAsync(orderId, updatePartialOrderRequest));
    }
    
    [Fact]
    public async Task RemoveOrderByIdAsync_ShouldRemoveOrder()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();

        _mockOrderRepository.Setup(r => r.RemoveOrderByIdAsync(orderId))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.RemoveOrderByIdAsync(orderId);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }
    
    [Fact]
    public async Task AddOrderAsync_ShouldAddOrder()
    {
        // Arrange
        var userId = 0;
        var createOrderRequest = new CreateOrderRequest();
        var printOrder = new PrintOrder();
        var printOrderDto = new PrintOrderDto();

        _mockMapper.Setup(m => m.Map<CreateOrderRequest, PrintOrder>(It.IsAny<CreateOrderRequest>(), It.IsAny<Action<IMappingOperationOptions>>()))
            .Returns(printOrder);
        _mockOrderRepository.Setup(r => r.CreateOrderAsync(It.IsAny<PrintOrder>()))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.AddOrderAsync(userId, createOrderRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }
    
    [Fact]
    public async Task RemoveOrderByIdAsync_ShouldThrowNotFoundOrderInDbException()
    {
        // Arrange
        var orderId = 1;

        _mockOrderRepository.Setup(r => r.RemoveOrderByIdAsync(orderId))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.RemoveOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldReturnOrder()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder()
        {
            PrintOrderId = orderId,
            Printer = new Printer(),
            PrinterId = 1
        };
        var printOrderDto = new PrintOrderDto();
    
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
        _mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync(printOrder);
    
        // Act
        var result = await _orderService.GetOrderByIdAsync(orderId);
    
        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task GetOrderByIdAsync_ShouldThrowNotFoundOrderInDbException()
    {
        // Arrange
        var orderId = 1;
        var printOrderDto = new PrintOrderDto();
    
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
       _mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.GetOrderByIdAsync(orderId));
    }
    
    [Fact]
    public async Task AbortOrderById_ShouldAbortOrder()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder{ PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();
    
        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);
    
        // Act
        var result = await _orderService.AbortOrderByIdAsync(orderId);
    
        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task AbortOrderById_ShouldThrowNotFoundInDbException()
    {
        // Arrange
        var orderId = 1;
        _mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync((PrintOrder) null);
        
        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.AbortOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task AbortOrderById_ShouldThrowInvalidOrderStatusException()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder{ PrintOrderStatusId = 2 };
        
        _mockOrderRepository.Setup(r => r.GetOrderById(It.IsAny<int>()))
            .ReturnsAsync(printOrder);
        
        // Act and Assert
        await Assert.ThrowsAsync<InvalidOrderStatusException>(() => _orderService.AbortOrderByIdAsync(orderId));
    }
    [Fact]
    public async Task AcceptOrderByIdAsync_ShouldAcceptOrder()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder { PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();

        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.AcceptOrderByIdAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task AcceptOrderByIdAsync_ShouldThrowNotFoundOrderInDbException()
    {
        // Arrange
        var orderId = 1;

        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync((PrintOrder)null);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.AcceptOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task AcceptOrderByIdAsync_ShouldThrowInvalidOrderStatusException()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder { PrintOrderStatusId = 2 };

        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync(printOrder);

        // Act and Assert
        await Assert.ThrowsAsync<InvalidOrderStatusException>(() => _orderService.AcceptOrderByIdAsync(orderId));
    }
    [Fact]
    public async Task DeclineOrderByIdAsync_ShouldDeclineOrder()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder { PrintOrderStatusId = 1 };
        var printOrderDto = new PrintOrderDto();

        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync(printOrder);
        _mockOrderRepository.Setup(r => r.UpdateOrderAsync(orderId, printOrder))
            .ReturnsAsync(printOrder);
        _mockMapper.Setup(m => m.Map<PrintOrderDto>(It.IsAny<PrintOrder>()))
            .Returns(printOrderDto);

        // Act
        var result = await _orderService.DeclineOrderByIdAsync(orderId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(printOrderDto, result);
    }

    [Fact]
    public async Task DeclineOrderByIdAsync_ShouldThrowNotFoundOrderInDbException()
    {
        // Arrange
        var orderId = 1;

        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync((PrintOrder)null);

        // Act and Assert
        await Assert.ThrowsAsync<NotFoundOrderInDbException>(() => _orderService.DeclineOrderByIdAsync(orderId));
    }

    [Fact]
    public async Task DeclineOrderByIdAsync_ShouldThrowInvalidOrderStatusException()
    {
        // Arrange
        var orderId = 1;
        var printOrder = new PrintOrder { PrintOrderStatusId = 2 };

        _mockOrderRepository.Setup(r => r.GetOrderById(orderId))
            .ReturnsAsync(printOrder);

        // Act and Assert
        await Assert.ThrowsAsync<InvalidOrderStatusException>(() => _orderService.DeclineOrderByIdAsync(orderId));
    }
}