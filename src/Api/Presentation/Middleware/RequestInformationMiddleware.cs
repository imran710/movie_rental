using System.Diagnostics;

namespace Api.Presentation.Middleware;

public partial class RequestInformationMiddleware(RequestDelegate next, ILogger<RequestInformationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = Stopwatch.GetTimestamp();
        await next(context);
        LogRequestInformation(
            context.Request.Method,
            context.Request.Path,
            context.Response.StatusCode,
            Stopwatch.GetElapsedTime(startTime).TotalMilliseconds);
    }

    [LoggerMessage(
        Level = LogLevel.Information,
        Message = "`{RequestMethod}` `{RequestUrl}` `{ResponseStatus}` `{RequestTime}`ms")]
    public partial void LogRequestInformation(string requestMethod, string requestUrl, int responseStatus, double requestTime);
}
