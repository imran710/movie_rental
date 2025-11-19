using Polly;
using Polly.Registry;

namespace Core.Infrastructure.Resilience;

public static class ResilienceExtensions
{
    public static ResiliencePipeline GetPipelineDefault(this ResiliencePipelineProvider<string> pipelineProvider)
    {
        return pipelineProvider.GetPipeline(DefaultResilienceConstant.DefaultResilience);
    }
}
