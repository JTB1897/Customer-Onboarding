# Configuration

## Environment Setup

### .NET Backend

Database path in `Program.cs`:
```csharp
var dbPath = Path.Combine(AppContext.BaseDirectory, "customers.db");
```

Change it to use a custom location:
```csharp
var dbPath = "C:\\Data\\customers.db";
```

Or via environment variable:
```csharp
var dbPath = Environment.GetEnvironmentVariable("DB_PATH") 
    ?? Path.Combine(AppContext.BaseDirectory, "customers.db");
```

### API Port

Default is 5000. To change, edit `Program.cs`:
```csharp
app.Urls.Add("http://localhost:7000");
```

### CORS

Currently allows any origin. For production, restrict it:
```csharp
policy => policy
    .WithOrigins("https://yourdomain.com")
    .AllowAnyMethod()
    .AllowAnyHeader()
```

### Frontend API URL

In `frontend/src/App.js`:
```javascript
const API_BASE_URL = 'http://localhost:5000/api';
```

For production:
```javascript
const API_BASE_URL = 'https://api.yourdomain.com/api';
```

## Build & Deploy

### Backend Release Build

```bash
cd src/Api
dotnet publish -c Release -o publish
```

### Frontend Production Build

```bash
cd frontend
npm run build
```

Creates optimized build in `frontend/build/`.

## Database Backup

Just copy the `.db` file:
```bash
copy customers.db customers.db.backup
```

## Logging

Default .NET logging sends to console. Can add file logging via Serilog if needed.

## Docker

Not set up yet, but if you need it, the basic setup would be:

### Backend Dockerfile
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY src/Api .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Api.dll"]
```

### Frontend Dockerfile
```dockerfile
FROM node:18 AS build
WORKDIR /app
COPY frontend .
RUN npm install && npm run build

FROM node:18
WORKDIR /app
RUN npm install -g serve
COPY --from=build /app/build .
EXPOSE 3000
CMD ["serve", "-s", ".", "-l", "3000"]
```

## Deployment Checklist

- [ ] Update API_BASE_URL in frontend
- [ ] Set CORS to production domain
- [ ] Enable HTTPS
- [ ] Set ASPNETCORE_ENVIRONMENT=Production
- [ ] Configure database for persistence
- [ ] Test end-to-end
- [ ] Set up backups
- [ ] Monitor logs

