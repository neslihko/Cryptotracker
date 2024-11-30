namespace Cryptotracker.Shared.Constants;

public static class JobConstants
{
    public static class RetryPolicy
    {
        public const int MaxRetryAttempts = 3;
        public const int BaseDelaySeconds = 2;
    }

    public static class Logging
    {
        public const string JobStartMessage = "Starting crypto data fetch at: {CurrentTime}. Next run scheduled for: {NextRun}.Fetching from: {Endpoint}";
        public const string RetryMessageWithoutException = "Retry attempt {0} failed after {1} seconds.";
        public const string RetryMessage = "Retry attempt {0} failed after {1} seconds. Error: {2}";

        public const string DataReceivedMessage = "Received data for {Count} cryptocurrencies";
        public const string JobCompletedMessage = "Crypto data updated successfully. API fetch took {FetchTime}ms, database update took {UpdateTime}ms, total job time: {TotalTime}ms";
        public const string RateLimitMessage = "Rate limit exceeded after retries. Job will be rescheduled.";
        public const string CriticalErrorMessage = "Critical error occurred while fetching crypto data";
        public const string InnerErrorMessage = "Inner exception: {InnerError}";
    }

    public static class Errors
    {
        public const string NoDataReceived = "No crypto data received from API";
    }
}