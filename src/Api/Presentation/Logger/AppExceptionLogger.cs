using Core.Domain.Logger;

namespace Api.Presentation.Logger;

public static partial class AppExceptionLogger
{
    [LoggerMessage(
        Level = LogLevel.Critical,
        Message = "App Exception Occurred. Message: `{AppExceptionMessage}`, Request path: `{AppExceptionRequestPath}`, Request body: `{AppExceptionRequestBody}`")]
    public static partial void LogAppException(
        this ILogger<StaticLogger> logger,
        string appExceptionMessage,
        string appExceptionRequestPath,
        string appExceptionRequestBody = "");
}
