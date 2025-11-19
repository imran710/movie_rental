namespace Api.Presentation.Response;

public class ApiResponse<T>
{
    public required string Message { get; init; }
    public int Status { get; init; }
    public T? Data { get; init; } = default;
    public ValidationErrors Errors { get; init; } = ApiResponseBuilder.Errors;
}

public static class ApiResponseBuilder
{
    public static readonly ValidationErrors Errors = [];

    public static ApiResponse<T> Create<T>(
        string message,
        int statusCode,
        T? data = default,
        ValidationErrors? errors = null)
    {
        return new ApiResponse<T>()
        {
            Message = message,
            Status = statusCode,
            Data = data,
            Errors = errors ?? Errors,
        };
    }
}
