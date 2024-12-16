﻿namespace Cryptotracker.Shared.Exceptions;

public class RateLimitExceededException : Exception
{
    public RateLimitExceededException(string message)
        : base(message) { }

    public RateLimitExceededException(string message, Exception innerException)
        : base(message, innerException) { }
}