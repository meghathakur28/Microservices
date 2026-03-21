using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExternalService>();
// Add services to the container.

builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddSingleton<RabbitMqService>();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
