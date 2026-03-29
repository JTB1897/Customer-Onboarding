using Application.Services;
using Infrastructure.Data;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(x => x.AddPolicy("dev", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var dbPath = Path.Combine(AppContext.BaseDirectory, "customers.db");
builder.Services.AddDbContext<CustomerDbContext>(x => x.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IValidationService, ValidationService>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CustomerDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("dev");
app.MapControllers();

app.Run();
