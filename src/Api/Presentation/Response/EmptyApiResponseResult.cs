using Api.Presentation.Logger;

using Core.Domain.Logger;

using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Presentation.Response;

public static class EmptyApiResponseResult
{
    private readonly static ILogger<StaticLogger> Logger = StaticLogger.Logger;

    public static Ok<EmptyApiResponse> Success<T>(Result<Success<T>> result)
    {
        return TypedResults.Ok(EmptyApiResponseBuilder.Create(result.Value.Message, StatusCodes.Status200OK));
    }

    public static Ok<EmptyApiResponse> Success(string title = "Success")
    {
        return TypedResults.Ok(EmptyApiResponseBuilder.Create(title, StatusCodes.Status200OK));
    }

    public static JsonHttpResult<EmptyApiResponse> Problem(
        string message = "Error occurred",
        int statusCode = StatusCodes.Status400BadRequest)
    {
        return TypedResults.Json(EmptyApiResponseBuilder.Create(message, statusCode), statusCode: statusCode);
    }

    public static JsonHttpResult<EmptyApiResponse> Problem(Error error)
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
            EmptyApiResponseBuilder.Create(error.Message, statusCode, errors: error.ValidationErrors),
            statusCode: statusCode);
    }
    
    public static JsonHttpResult<EmptyApiResponse> Problem<T>(Result<T> result)
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
            EmptyApiResponseBuilder.Create(result.Error.Message, statusCode, errors: result.Error.ValidationErrors),
            statusCode: statusCode);
    }
}
