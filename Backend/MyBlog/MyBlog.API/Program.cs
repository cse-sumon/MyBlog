using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyBlog.Infrastructure.Data;
using MyBlog.Application.Interfaces.Repositories;
using MyBlog.Application.Interfaces.Services;
using MyBlog.Infrastructure.Repositories;
using MyBlog.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register EF Core DbContext using connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Register application services and repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
   // app.MapOpenApi();
    app.UseSwagger();               // Swagger JSON endpoint
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBlog API v1");
        c.RoutePrefix = ""; // Swagger at root URL: http://localhost:5000/
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Redirect root URL to the API default route so opening https://localhost:.../ goes to /api/home
//app.MapGet("/", () => Results.Redirect("/api/home"));

app.MapControllers();

app.Run();
