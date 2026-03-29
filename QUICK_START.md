# Quick Start

## What's Here

Full-stack customer onboarding app. Backend is .NET 8, frontend is React.

## What You Need

- .NET 8 SDK
- Node.js (LTS)
- Two terminal windows

## Run It

### Terminal 1: Backend

```powershell
cd src/Api
dotnet restore
dotnet run
```

Listens on http://localhost:5000

### Terminal 2: Frontend

```powershell
cd frontend
npm install
npm start
```

Opens at http://localhost:3000

## Try It Out

1. Go to http://localhost:3000
2. Fill in a customer name, email, phone
3. Draw a signature on the canvas
4. Click submit
5. See it appear in the customer list

## API

Test endpoints at http://localhost:5000/swagger

- `POST /api/customers` - New customer
- `GET /api/customers` - List all
- `GET /api/customers/{id}` - Get one

## Tests

```powershell
cd src/Tests
dotnet test
```

9 unit tests covering validation and service logic.

## If Something Breaks

**Backend won't start**
- Check port 5000 is free
- .NET 8 installed? `dotnet --version`

**Frontend shows errors**
- Is backend running?
- Check browser console (F12)

**Database issues**
- Delete `customers.db` and restart

