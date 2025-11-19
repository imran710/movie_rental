using Core.Domain.Logger;

using Microsoft.Extensions.Logging;

namespace Core.Infrastructure.Common.Logger;

public static partial class OptionValidateLogger
{
    [LoggerMessage(
        Level = LogLevel.Critical,
        Message = "Option validation failed! Option name: `{OptionName}` Message: `{OptionValidationMessage}`")]
    public static partial void OptionValidationFailed(this ILogger<StaticLogger> logger, string optionName, string optionValidationMessage);
}
