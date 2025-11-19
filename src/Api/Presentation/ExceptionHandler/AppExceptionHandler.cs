using Api.Presentation.Response;
using System.Data.Common;
using System.Data;

using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;
using Core.Domain.Logger;
using Api.Presentation.Logger;
using Core.Domain.Exceptions;
using Core.Features.Common.Service;

namespace Api.Presentation.ExceptionHandler;

public class AppExceptionHandler(IWebHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var isDevelopment = env.IsDevelopment();

        if (exception is DomainException domainException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(EmptyApiResponseBuilder.Create(
                domainException.Error.Message,
                StatusCodes.Status400BadRequest,
                domainException.Error.ValidationErrors), cancellationToken).ConfigureAwait(false);

            return true;
        }

        var (statusCode, errorMessage) = exception switch
        {
            // Common
            FormatException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Unable to format data"),
            InvalidCastException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Unable to cast from one type to another type"),
            NotImplementedException => (StatusCodes.Status501NotImplemented, "Feature is not implemented"),
            NotSupportedException => (StatusCodes.Status500InternalServerError, "Feature is not supported"),
            ArgumentException e => (StatusCodes.Status501NotImplemented, isDevelopment ? e.Message : "Invalid argument is passed"),
            TimeoutException => (StatusCodes.Status504GatewayTimeout, "Operation timed out"),
            OperationCanceledException => (StatusCodes.Status500InternalServerError, "Operation cancelled"),
            PermissionException e => (StatusCodes.Status403Forbidden, e.Message),

            // DB Related
            DBConcurrencyException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Failed to save, item is already changed by other user"),
            DbException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Database error occurred"),

            // JSON
            JsonException => (StatusCodes.Status400BadRequest, "Invalid JSON format"),

            // Memory
            InsufficientMemoryException e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Insufficient memory, please contact system administrator"),

            // IO Related
            UnauthorizedAccessException => (StatusCodes.Status500InternalServerError, "Does not have enough permission to access file/folder"),
            PathTooLongException => (StatusCodes.Status500InternalServerError, "Path is too long"),
            DirectoryNotFoundException => (StatusCodes.Status500InternalServerError, "Directory is not found"),
            IOException => (StatusCodes.Status500InternalServerError, "Unknown I/O exception occurred"),

            Exception e => (StatusCodes.Status500InternalServerError, isDevelopment ? e.Message : "Unknown error has occurred"),
            _ => (StatusCodes.Status500InternalServerError, "Unknown error has occurred")
        };

        if (httpContext.Request.ContentLength > 0)
        {
            StaticLogger.Logger.LogAppException(exception.Message, httpContext.Request.Path, string.Empty);
        }
        else
        {
            StaticLogger.Logger.LogAppException(exception.Message, httpContext.Request.Path);
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(EmptyApiResponseBuilder.Create(errorMessage, statusCode), cancellationToken).ConfigureAwait(false);

        return true;
    }
}
