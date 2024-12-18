using Cryptotracker.Shared.Configuration;
using Cryptotracker.Shared.Data;
using Cryptotracker.Shared.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using System.Net.Http.Headers;

var builder = Host.CreateApplicationBuilder(args);

// Add configuration
builder.Services.Configure<ApiSettings>(
    builder.Configuration.GetSection("ApiSettings"));

// Add database context
builder.Services.AddDbContext<CryptoDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services
builder.Services.AddScoped<IDatabaseService, DatabaseService>();

// Register HTTP client with settings
builder.Services.AddHttpClient<ICryptoApiClient, CoinCapApiClient>((serviceProvider, client) =>
{
    var settings = builder.Configuration
        .GetSection("ApiSettings:CoinCap")
        .Get<CoinCapSettings>()
        ?? throw new InvalidOperationException("CoinCap settings not configured");
    client.BaseAddress = new Uri(settings.BaseUrl);
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

// Configure Quartz
builder.Services.AddQuartz(q =>
{
    var settings = builder.Configuration
        .GetSection("ApiSettings:CoinCap")
        .Get<CoinCapSettings>()
        ?? throw new InvalidOperationException("CoinCap settings not configured");

    // Register the job
    _ = q.AddJob<CryptoDataFetchJob>(opts => opts
        .WithIdentity("CryptoDataFetchJob")
        .DisallowConcurrentExecution());

    // Create a trigger
    _ = q.AddTrigger(opts => opts
        .ForJob("CryptoDataFetchJob")
        .WithIdentity("CryptoDataFetchJob-trigger")
        .WithCronSchedule($"0 0/{settings.UpdateIntervalMinutes} * * * ?")); // Cron expression for every X minutes
});

// Add the Quartz hosted service
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
    options.AwaitApplicationStarted = true;
});

var app = builder.Build();

// Remove the database migration check entirely since we don't need it
// Just validate configuration
var settings = app.Services.GetRequiredService<IOptions<ApiSettings>>().Value;
ConfigurationValidator.ValidateApiSettings(settings);

await app.RunAsync();