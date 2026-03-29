using Application.DTOs;
using Application.Services;
using Xunit;

namespace Tests;

public class ValidationServiceTests
{
    private readonly IValidationService _validationService;

    public ValidationServiceTests()
    {
        _validationService = new ValidationService();
    }

    [Fact]
    public void ValidateCustomer_WithValidData_DoesNotThrow()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "09551234567"
        };

        _validationService.ValidateCustomer(request);
    }

    [Fact]
    public void ValidateCustomer_WithMissingFirstName_ThrowsException()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "09551234567"
        };

        var ex = Assert.Throws<ValidationException>(() => _validationService.ValidateCustomer(request));
        Assert.Contains("First name is required", ex.Message);
    }

    [Fact]
    public void ValidateCustomer_WithInvalidEmail_ThrowsException()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "invalid-email",
            PhoneNumber = "09551234567"
        };

        var ex = Assert.Throws<ValidationException>(() => _validationService.ValidateCustomer(request));
        Assert.Contains("Valid email is required", ex.Message);
    }

    [Fact]
    public void ValidateCustomer_WithShortPhoneNumber_ThrowsException()
    {
        var request = new CreateCustomerRequest
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com",
            PhoneNumber = "123"
        };

        var ex = Assert.Throws<ValidationException>(() => _validationService.ValidateCustomer(request));
        Assert.Contains("Valid phone number is required", ex.Message);
    }
}
