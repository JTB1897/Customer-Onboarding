using Application.DTOs;
using System.Text.RegularExpressions;

namespace Application.Services;

public interface IValidationService
{
    void ValidateCustomer(CreateCustomerRequest request);
}

public class ValidationService : IValidationService
{
    public void ValidateCustomer(CreateCustomerRequest request)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.FirstName))
            errors.Add("First name is required");
        if (string.IsNullOrWhiteSpace(request.LastName))
            errors.Add("Last name is required");
        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            errors.Add("Valid email is required");
        if (string.IsNullOrWhiteSpace(request.PhoneNumber) || !IsValidPhoneNumber(request.PhoneNumber))
            errors.Add("Valid phone number is required");

        if (errors.Count > 0)
            throw new ValidationException(string.Join(", ", errors));
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidPhoneNumber(string phone)
    {
        var digitsOnly = Regex.Replace(phone, @"\D", "");
        return digitsOnly.Length >= 10;
    }
}

public class ValidationException : Exception
{
    public ValidationException(string message) : base(message) { }
}
