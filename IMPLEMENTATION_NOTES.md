# Implementation Notes

## Architecture

Standard clean architecture split into layers:

- **Domain**: Just entities, no dependencies
- **Application**: Business logic and interfaces
- **Infrastructure**: Database implementation
- **API**: Controllers and HTTP stuff

Each layer only depends on layers below it. Domain never depends on anything.

## Why This Layout

- Easy to test - mock the dependencies
- Easy to change - database can be swapped without touching business logic  
- Easier to reason about - you know where things are

## Design Patterns

### Repository Pattern

`ICustomerRepository` is an interface in the Application layer, implemented in Infrastructure. This lets us swap database implementations without changing the service.

### Service Layer

`CustomerService` handles business logic. It validates through `IValidationService` and fetches through `ICustomerRepository`. Both are injected.

### Dependency Injection 

Everything is registered in `Program.cs`. When Controller needs a service, it gets injected. No `new` keyword for dependencies.

### DTOs

`CreateCustomerRequest` and `CustomerResponse` separate the API contract from internal domain models. The API doesn't expose domain objects directly.

## Database

Using SQLite with Entity Framework Core. Auto-creates the schema on startup via `EnsureCreated()`.

Tables are created from the DbContext configuration in `CustomerDbContext`.

## Validation

Three levels:

1. **Frontend**: Real-time validation as you type
2. **Controller**: Model binding catches obvious issues  
3. **Service**: Business rules checked before persisting

Email validation uses `System.Net.Mail.MailAddress` for RFC compliance. Phone validation counts digits.

## Error Handling

- ValidationException for business logic errors
- Try/catch in controller logs errors and returns appropriate HTTP codes
- 400 for validation, 404 for missing, 500 for server errors

## Frontend

React with hooks. State managed locally with `useState`. API calls with `fetch`.

SignaturePad component handles canvas drawing. Uses Canvas 2D API directly. Exports as Base64 PNG.

## Testing

9 tests total. 4 for validation, 5 for service layer.

Tests use Moq to mock dependencies. `ValidationServiceTests` doesn't mock anything since it's just logic. `CustomerServiceTests` mocks the repository and logger.

## Things to Know

The code assumes you have .NET 8 and Node 16+ installed. Database is SQLite - file-based, no server needed.

Frontend runs on dev server (hot reload on save). Backend is a proper ASP.NET Core app.

CORS is wide open for local development. Restrict it in production to your domain.

## Future Stuff

If you need authentication, add it as middleware in `Program.cs`. If you need caching, use Response caching attributes on controllers. Pagination would be query parameters on the list endpoint.

Database indexing would help if the customer table gets huge. Add indexes to frequently queried columns (Email, PhoneNumber).

Logging is basic .NET logging. If you need structured logging, add Serilog.

## Code Organization

- One class per file (except DTOs which are grouped)
- Interfaces in Application layer where they're used
- Implementations in Infrastructure layer
- Async/await for all I/O operations

No XML comments - code is self-documenting through naming.

