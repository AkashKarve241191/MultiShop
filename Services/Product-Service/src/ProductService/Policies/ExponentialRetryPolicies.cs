namespace ProductService.Policies {
    using System;
    using Microsoft.Azure.ServiceBus;
    using Microsoft.Extensions.Logging;
    using Polly.Retry;
    using Polly;
    public class ExponentialRetryPolicies {
        private readonly ILogger<ExponentialRetryPolicies> _logger;
        public readonly AsyncRetryPolicy ExponentialRetryPolicy;

        public ExponentialRetryPolicies (ILogger<ExponentialRetryPolicies> logger) {
            _logger = logger;
            ExponentialRetryPolicy = Policy.Handle<Exception> ().WaitAndRetryAsync (3, attempt => TimeSpan.FromMilliseconds (100 * Math.Pow (2, attempt)), (ex,_) => _logger.LogError (ex.ToString ()));
        }
    }
}