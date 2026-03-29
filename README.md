# Customer Registration & Onboarding System

Full-stack customer registration and onboarding system with signature capture.

## Overview

- **Backend**: C#/.NET 8 REST API with SQLite
- **Frontend**: React with canvas signature capture
- **Tests**: xUnit + Moq

## Architecture

Clean architecture with separation of concerns:

```
/src
  /Api             - Controllers and endpoints
  /Application     - Business logic and services
  /Domain          - Entity models
  /Infrastructure  - Database and repositories
  /Tests           - Unit tests
/frontend          - React app
```

## Prerequisites

- .NET 8 SDK
- Node.js 16+
- npm

## Getting Started

### Backend

```powershell
cd src/Api
dotnet restore
dotnet run
```

Runs at `http://localhost:5000`. Check Swagger UI at `http://localhost:5000/swagger`.

### Frontend

```powershell
cd frontend
npm install
npm start
```

Runs at `http://localhost:3000`.

## API Endpoints

- `POST /api/customers` - Create customer
- `GET /api/customers/{id}` - Get customer by ID
- `GET /api/customers` - List all customers

### Example Request

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "5551234567",
  "signatureData": "data:image/png;base64,..."
}
```

### Example Response

```json
{
  "id": 1,
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phoneNumber": "5551234567",
  "signatureData": "data:image/png;base64,...",
  "dateCreated": "2026-03-28T10:30:00Z"
}
```

## Frontend Features

- Customer registration form with validation
- Canvas signature pad
- Live form validation
- Customer listing
- Success confirmation
- Canvas-based signature capture
- Real-time form validation
- Success confirmation after registration
- Customer listing with all registered customers
- Responsive design

## Project Structure

### Backend

```
src/Api/
├── Api/
│   └── CustomersController.cs
├── Application/
│   ├── DTOs.cs
│   ├── ValidationService.cs
│   └── CustomerService.cs
├── Domain/
│   └── Customer.cs
├── Infrastructure/
│   ├── CustomerDbContext.cs
│   └── CustomerRepository.cs
├── Program.cs
└── Api.csproj

src/Tests/
├── ValidationServiceTests.cs
├── CustomerServiceTests.cs
└── Tests.csproj
```

### Frontend

```
frontend/
├── public/index.html
├── src/
│   ├── App.js
│   ├── SignaturePad.js
│   ├── index.js
│   └── index.css
└── package.json
```

## Testing

```powershell
cd src/Tests
dotnet test
```

Runs 9 unit tests covering validation and business logic.

## Database

SQLite with auto-creation on first run. Database file created at `customers.db` in the bin directory.

## Validation

- First/Last Name: Required
- Email: Valid format
- Phone: Min 10 digits
- Signature: Optional

## Troubleshooting

**Backend won't start**
- Check .NET 8 is installed: `dotnet --version`
- Port 5000 free: `netstat -ano | findstr :5000`
- Clear build: `dotnet clean` && `dotnet run`

**Frontend connection error**
- Backend running on :5000?
- Check browser console (F12)
- CORS configured in Program.cs

**Database locked**
- Restart backend
- Delete `customers.db` if corrupted

## Tech Stack

- **Backend**: ASP.NET Core 8, EF Core, SQLite
- **Frontend**: React 18, Canvas API, Fetch
- **Tests**: xUnit, Moq

