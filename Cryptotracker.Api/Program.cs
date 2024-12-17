using Cryptotracker.Shared.Configuration;
using Cryptotracker.Shared.Data;
using Cryptotracker.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Cryptotracker API",
        Version = "v1",
        Description = "API for tracking cryptocurrency prices"
    });
});

// Configure API Settings
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

// Configure HttpClient
builder.Services.AddHttpClient<ICryptoApiClient, CoinCapApiClient>((serviceProvider, client) =>
{
    var settings = builder.Configuration
        .GetSection("ApiSettings:CoinCap")
        .Get<CoinCapSettings>();
    if (settings == null)
        throw new InvalidOperationException("CoinCap settings not configured");
    client.BaseAddress = new Uri(settings.BaseUrl);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

// Add database context
builder.Services.AddDbContext<CryptoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        _ = builder
            .WithOrigins(
                "https://sparentracker.netlify.app",
                "http://localhost:3000",
                "http://localhost:5173",
                "https://cryptotracker-dx91.onrender.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI(c =>
   {
       c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cryptotracker API V1");
       c.RoutePrefix = string.Empty;  // This makes Swagger UI the root page
   });
}

// Configure middleware pipeline
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors();
app.UseAuthorization();
app.MapControllers();



app.Run();