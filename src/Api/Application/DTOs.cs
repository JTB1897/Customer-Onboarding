namespace Application.DTOs;

public class CreateCustomerRequest
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? SignatureData { get; set; }
}

public class CustomerResponse
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PhoneNumber { get; set; }
    public string? SignatureData { get; set; }
    public DateTime DateCreated { get; set; }
}
