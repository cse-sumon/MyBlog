using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using MyBlog.Infrastructure.Data;
using MyBlog.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
// using MyBlog.API.Extensions; // removed - seeding handled by hosted service
using MyBlog.Application.Interfaces.Repositories;
using MyBlog.Application.Interfaces.Services;
using MyBlog.Infrastructure.Repositories;
using MyBlog.Application.Services;
using MyBlog.API.Middleware;
using Serilog;
using Serilog.Events;
using MyBlog.API.HostedServices;

// Configure Serilog before building the host
// Configure Serilog

var builder = WebApplication.CreateBuilder(args);

//Configure Serilog
var logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("Logs/MyBlog_log.txt", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Error()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


// Add services to the container.

builder.Services.AddControllers();

// Enable CORS for Angular dev server
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Register EF Core DbContext using connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure Identity
// Configure Identity with roles
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Add authentication with JWT
//var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key") ?? "ThisIsASecretKeyForDevOnlyReplaceInProduction";
//var jwtIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer") ?? "MyBlogApi";

var jwtKey = builder.Configuration["Jwt:Key"] ?? "ThisIsASecretKeyForDevOnlyReplaceInProduction";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "MyBlogApi";

var keyBytes = System.Text.Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keyBytes)
        };
    });

builder.Services.AddAuthorization();

// Register application services and repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Register startup seeder as a hosted service
builder.Services.AddHostedService<MyBlog.API.HostedServices.StartupSeeder>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();               // Swagger JSON endpoint
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBlog API v1");
        c.RoutePrefix = ""; 
    });
}

// Global exception middleware (should run early in the pipeline)
app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

// Use authentication & authorization
app.UseAuthentication();
app.UseAuthorization();

// Use CORS
app.UseCors("CorsPolicy");

// Redirect root URL to the API default route so opening https://localhost:.../ goes to /api/home
//app.MapGet("/", () => Results.Redirect("/api/home"));

app.MapControllers();

app.Run();
