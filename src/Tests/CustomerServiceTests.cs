using Application.DTOs;
using Application.Services;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Tests;

public class CustomerServiceTests
{
    private readonly Mock<ICustomerRepository> _mockRepository;
    private readonly Mock<IValidationService> _mockValidationService;
    private readonly Mock<ILogger<CustomerService>> _mockLogger;
    private readonly ICustomerService _customerService;

    public CustomerServiceTests()
    {
        _mockRepository = new Mock<ICustomerRepository>();
        _mockValidationService = new Mock<IValidationService>();
        _mockLogger = new Mock<ILogger<CustomerService>>();

        _customerService = new CustomerService(
            _mockRepository.Object,
            _mockValidationService.Object,
            _mockLogger.Object);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithValidRequest_CreatesCustomer()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "5551234567"
        };

        _mockValidationService.Setup(v => v.ValidateCustomer(request));

        var customer = new Customer
        {
            Id = 1,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            DateCreated = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.CreateAsync(It.IsAny<Customer>()))
            .ReturnsAsync(customer);

        var result = await _customerService.CreateCustomerAsync(request);

        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
        Assert.Equal("john@example.com", result.Email);
        _mockRepository.Verify(r => r.CreateAsync(It.IsAny<Customer>()), Times.Once);
    }

    [Fact]
    public async Task CreateCustomerAsync_WithInvalidRequest_ThrowsException()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "5551234567"
        };

        _mockValidationService.Setup(v => v.ValidateCustomer(request))
            .Throws(new ValidationException("First name is required"));

        await Assert.ThrowsAsync<ValidationException>(() =>
            _customerService.CreateCustomerAsync(request));
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithValidId_ReturnsCustomer()
    {
        var customerId = 1;
        var customer = new Customer
        {
            Id = customerId,
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "5551234567",
            DateCreated = DateTime.UtcNow
        };

        _mockRepository.Setup(r => r.GetByIdAsync(customerId))
            .ReturnsAsync(customer);

        var result = await _customerService.GetCustomerByIdAsync(customerId);

        Assert.NotNull(result);
        Assert.Equal("John", result.FirstName);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_WithInvalidId_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Customer?)null);

        var result = await _customerService.GetCustomerByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ReturnsAllCustomers()
    {
        var customers = new List<Customer>
        {
            new Customer
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                PhoneNumber = "5551234567",
                DateCreated = DateTime.UtcNow
            },
            new Customer
            {
                Id = 2,
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane@example.com",
                PhoneNumber = "5559876543",
                DateCreated = DateTime.UtcNow
            }
        };

        _mockRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(customers);

        var result = await _customerService.GetAllCustomersAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }
}
