using Application.DTOs;
using Domain.Entities;

namespace Application.Services;

public interface ICustomerService
{
    Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request);
    Task<CustomerResponse?> GetCustomerByIdAsync(int id);
    Task<List<CustomerResponse>> GetAllCustomersAsync();
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IValidationService _validationService;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        ICustomerRepository repository,
        IValidationService validationService,
        ILogger<CustomerService> logger)
    {
        _repository = repository;
        _validationService = validationService;
        _logger = logger;
    }

    public async Task<CustomerResponse> CreateCustomerAsync(CreateCustomerRequest request)
    {
        _validationService.ValidateCustomer(request);

        var customer = new Customer
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            SignatureData = request.SignatureData,
            DateCreated = DateTime.UtcNow
        };

        var createdCustomer = await _repository.CreateAsync(customer);
        _logger.LogInformation("Customer created: {CustomerId}", createdCustomer.Id);

        return MapToResponse(createdCustomer);
    }

    public async Task<CustomerResponse?> GetCustomerByIdAsync(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer != null ? MapToResponse(customer) : null;
    }

    public async Task<List<CustomerResponse>> GetAllCustomersAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(MapToResponse).ToList();
    }

    private static CustomerResponse MapToResponse(Customer customer)
    {
        return new CustomerResponse
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            SignatureData = customer.SignatureData,
            DateCreated = customer.DateCreated
        };
    }
}

public interface ICustomerRepository
{
    Task<Customer> CreateAsync(Customer customer);
    Task<Customer?> GetByIdAsync(int id);
    Task<List<Customer>> GetAllAsync();
}
