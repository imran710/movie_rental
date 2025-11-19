using Api.Presentation.Logger;

using Core.Domain.Logger;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Presentation.Response;

public static partial class ApiResponseResult
{
    private readonly static ILogger<StaticLogger> Logger = StaticLogger.Logger;

    public static Ok<ApiResponse<T>> Success<T>(Result<T> data, string message = "Success")
    {
        return TypedResults.Ok(ApiResponseBuilder.Create(message, StatusCodes.Status200OK, data.Value));
    }

    public static Ok<ApiResponse<T>> Success<T>(T? data = default, string message = "Success")
    {
        return TypedResults.Ok(ApiResponseBuilder.Create(message, StatusCodes.Status200OK, data));
    }

    public static Ok<ApiResponse<T>> Success<T>(Result<Success<T>> result)
    {
        return Success<T>(result.Value.Data, result.Value.Message);
    }

    public static Ok<ApiResponse<T>> Success<T>(Success<T> success)
    {
        return Success<T>(success.Data, success.Message);
    }

    public static JsonHttpResult<ApiResponse<T>> Problem<T>(
        string message = "Error occurred",
        int statusCode = StatusCodes.Status400BadRequest,
        T? data = default)
    {
        return TypedResults.Json(
            ApiResponseBuilder.Create(message, statusCode, data),
            statusCode: statusCode);
    }

    public static JsonHttpResult<ApiResponse<T>> Problem<T>(
        Error error,
        T? data = default)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        Logger.LogRequestWithErrorCode(error.Code, error.Type);

        return TypedResults.Json(
            ApiResponseBuilder.Create(error.Message, statusCode, data, errors: error.ValidationErrors),
            statusCode: statusCode);
    }

    public static JsonHttpResult<ApiResponse<T>> Problem<T>(
        Result<Success<T>> result,
        T? data = default)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        Logger.LogRequestWithErrorCode(result.Error.Code, result.Error.Type);

        return TypedResults.Json(
            ApiResponseBuilder.Create(result.Error.Message, statusCode, data, errors: result.Error.ValidationErrors),
            statusCode: statusCode);
    }
    
    public static JsonHttpResult<ApiResponse<T>> Problem<T>(
        Result<T> result,
        T? data = default)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Failure => StatusCodes.Status400BadRequest,
            ErrorType.Unexpected => StatusCodes.Status500InternalServerError,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        Logger.LogRequestWithErrorCode(result.Error.Code, result.Error.Type);

        return TypedResults.Json(
            ApiResponseBuilder.Create(result.Error.Message, statusCode, data, errors: result.Error.ValidationErrors),
            statusCode: statusCode);
    }
}
