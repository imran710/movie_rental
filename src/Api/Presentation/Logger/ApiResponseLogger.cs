using Core.Domain.Logger;

namespace Api.Presentation.Logger;

public static partial class ApiResponseLogger
{
    [LoggerMessage(
        Level = LogLevel.Error,
        Message = "Response error. Code: `{RequestErrorCode}` Error type: `{RequestErrorType}`")]
    public static partial void LogRequestWithErrorCode(this ILogger<StaticLogger> logger, string requestErrorCode, ErrorType requestErrorType);
}
