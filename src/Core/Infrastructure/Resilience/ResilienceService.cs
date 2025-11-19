using Core.Infrastructure.ServiceInjector;

using Microsoft.Extensions.DependencyInjection;

using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;

namespace Core.Infrastructure.Resilience;

public class ResilienceService : IInjectServices
{
    public void Configure(IServiceCollection services)
    {
        services.AddResiliencePipeline(DefaultResilienceConstant.DefaultResilience, options => options.AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<Exception>(),
            Delay = TimeSpan.FromSeconds(DefaultResilienceConstant.DefaultResilienceDelayInSeconds),
            MaxRetryAttempts = DefaultResilienceConstant.DefaultResilienceMaxRetryAttempts,
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
        })
        .AddTimeout(TimeSpan.FromSeconds(DefaultResilienceConstant.DefaultResilienceTimeout))
        .AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            FailureRatio = 0.5,
            SamplingDuration = TimeSpan.FromSeconds(10),
            MinimumThroughput = 8,
            BreakDuration = TimeSpan.FromSeconds(25),
        }));
    }
}
