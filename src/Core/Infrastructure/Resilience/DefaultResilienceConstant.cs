namespace Core.Infrastructure.Resilience;

public static class DefaultResilienceConstant
{
    #region Default Resilience
    public const string DefaultResilience = "default";
    public const int DefaultResilienceDelayInSeconds = 1;
    public const int DefaultResilienceMaxRetryAttempts = 2;
    public const int DefaultResilienceTimeout = 30;
    #endregion
}
