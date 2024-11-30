using Cryptotracker.Shared.Configuration;
using Cryptotracker.Shared.Constants;
using Cryptotracker.Shared.Dto;
using Cryptotracker.Shared.Exceptions;
using Cryptotracker.Shared.Services;
using Microsoft.Extensions.Options;
using Polly;
using Quartz;

[DisallowConcurrentExecution]
public class CryptoDataFetchJob : IJob
{
    private readonly ICryptoApiClient _apiClient;
    private readonly IDatabaseService _databaseService;
    private readonly ILogger<CryptoDataFetchJob> _logger;
    private readonly IAsyncPolicy<IEnumerable<CryptoDto>> _retryPolicy;
    private readonly CoinCapSettings _settings;

    public CryptoDataFetchJob(
          ICryptoApiClient apiClient,
          IDatabaseService databaseService,
          ILogger<CryptoDataFetchJob> logger,
          IOptions<CoinCapSettings> settings)
    {
        _apiClient = apiClient;
        _databaseService = databaseService;
        _logger = logger;
        _settings = settings.Value;

        _retryPolicy = CreateRetryPolicy();
    }

    private IAsyncPolicy<IEnumerable<CryptoDto>> CreateRetryPolicy()
    {
        return Policy<IEnumerable<CryptoDto>>
            .Handle<HttpRequestException>()
            .Or<RateLimitExceededException>()
            .OrResult(r => !r.Any())
            .WaitAndRetryAsync(
                JobConstants.RetryPolicy.MaxRetryAttempts,
                retryAttempt => TimeSpan.FromSeconds(
                    Math.Pow(JobConstants.RetryPolicy.BaseDelaySeconds, retryAttempt)),
                (outcome, timeSpan, retryCount, _) =>
                {
                    if (outcome.Exception != null)
                    {
                        _logger.LogWarning(
                            outcome.Exception,
                            JobConstants.Logging.RetryMessage,
                            retryCount,
                            timeSpan.TotalSeconds,
                            outcome.Exception.Message);
                    }
                    else
                    {
                        _logger.LogWarning(
                            JobConstants.Logging.RetryMessageWithoutException,
                            retryCount,
                            timeSpan.TotalSeconds);
                    }
                });
    }

    public async Task Execute(IJobExecutionContext context)
    {
        var executionMetrics = new JobExecutionMetrics();

        try
        {
            executionMetrics.JobStartTime = DateTime.UtcNow;
            var nextFireTime = context.NextFireTimeUtc?.LocalDateTime;
            var endpoint = $"{_settings.BaseUrl}{_settings.AssetsEndpoint}";

            _logger.LogInformation(
                JobConstants.Logging.JobStartMessage,
                executionMetrics.JobStartTime,
                nextFireTime,
                endpoint);

            var cryptoData = await _retryPolicy.ExecuteAsync(async () =>
            {
                var data = await _apiClient.GetCurrentPrices();
                if (!data.Any())
                {
                    throw new InvalidOperationException(JobConstants.Errors.NoDataReceived);
                }
                return data;
            });

            _logger.LogInformation(
                JobConstants.Logging.DataReceivedMessage,
                cryptoData.Count());

            executionMetrics.UpdateStartTime = DateTime.UtcNow;
            await _databaseService.UpdatePrices(cryptoData);
            executionMetrics.CalculateMetrics();

            LogSuccessMetrics(executionMetrics);
            StoreMetricsInContext(context, executionMetrics, cryptoData.Count());
        }
        catch (RateLimitExceededException ex)
        {
            _logger.LogError(ex, JobConstants.Logging.RateLimitMessage);
            throw;
        }
        catch (Exception ex)
        {
            HandleJobException(ex, context);
            throw;
        }
    }

    private void LogSuccessMetrics(JobExecutionMetrics metrics)
    {
        _logger.LogInformation(
            JobConstants.Logging.JobCompletedMessage,
            metrics.FetchDuration.TotalMilliseconds,
            metrics.UpdateDuration.TotalMilliseconds,
            metrics.TotalDuration.TotalMilliseconds);
    }

    private void StoreMetricsInContext(
        IJobExecutionContext context,
        JobExecutionMetrics metrics,
        int itemCount)
    {
        context.Result = new JobResult
        {
            CryptocurrenciesUpdated = itemCount,
            FetchDurationMs = metrics.FetchDuration.TotalMilliseconds,
            UpdateDurationMs = metrics.UpdateDuration.TotalMilliseconds,
            TotalDurationMs = metrics.TotalDuration.TotalMilliseconds
        };
    }

    private void HandleJobException(Exception ex, IJobExecutionContext context)
    {
        _logger.LogError(ex, JobConstants.Logging.CriticalErrorMessage);

        if (ex.InnerException != null)
        {
            _logger.LogError(
                JobConstants.Logging.InnerErrorMessage,
                ex.InnerException.Message);
        }

        context.Result = new JobResult
        {
            Error = ex.Message,
            Timestamp = DateTime.UtcNow
        };
    }
}

public class JobExecutionMetrics
{
    public DateTime JobStartTime { get; set; }
    public DateTime UpdateStartTime { get; set; }
    public TimeSpan FetchDuration => UpdateStartTime - JobStartTime;
    public TimeSpan UpdateDuration { get; private set; }
    public TimeSpan TotalDuration { get; private set; }

    public void CalculateMetrics()
    {
        var endTime = DateTime.UtcNow;
        UpdateDuration = endTime - UpdateStartTime;
        TotalDuration = endTime - JobStartTime;
    }
}

public class JobResult
{
    public int CryptocurrenciesUpdated { get; set; }
    public double FetchDurationMs { get; set; }
    public double UpdateDurationMs { get; set; }
    public double TotalDurationMs { get; set; }
    public string? Error { get; set; }
    public DateTime? Timestamp { get; set; }
}