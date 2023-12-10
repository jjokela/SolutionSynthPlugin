using Polly;
using Polly.Retry;

namespace SolutionSynthPlugin.Settings;

public class PolicySettings
{
    public static ResiliencePipeline? GetResiliencePipeline()
    {
        return new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                BackoffType = DelayBackoffType.Constant,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(10),
            })
            .Build();
    }
}