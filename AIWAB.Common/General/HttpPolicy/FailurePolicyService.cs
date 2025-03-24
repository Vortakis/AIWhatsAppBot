
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace AIWAB.Common.General.HttpPolicy
{
    public class FailurePolicyService : IFailurePolicyService
    {
        private readonly ILogger<FailurePolicyService> _logger;

        public FailurePolicyService(ILogger<FailurePolicyService> logger) 
        {
            _logger = logger;
        }

        public IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError() 
                .OrResult(response => (int)response.StatusCode == 429) 
                .WaitAndRetryAsync(
                    3, 
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                    (response, timeSpan, retryCount, context) =>
                    {
                        _logger.LogWarning($"Retry {retryCount} after {timeSpan.TotalSeconds}s due to {response.Result?.StatusCode}");
                    });
        }

        public IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(
                    5, 
                    TimeSpan.FromMinutes(1),
                    onBreak: (outcome, timespan) => _logger.LogWarning($"Circuit broken! Retrying after {timespan.TotalSeconds}s"),
                    onReset: () => _logger.LogWarning("Circuit reset! API calls resuming."),
                    onHalfOpen: () => _logger.LogWarning("Circuit in half-open state. Testing request...")
                );
        }
    }
}
