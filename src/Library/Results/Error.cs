using Library.Values;

namespace Library.Results;

public readonly record struct Error
{
    public const string GeneralErrorCode = "Error.General.Unknown";
    private static readonly ValidationErrors EmptyValidationErrors = [];

    public string Message { get; init; }
    public ErrorType Type { get; init; }
    public string Code { get; init; }
    public readonly ValidationErrors ValidationErrors { get; init; } = EmptyValidationErrors;

    private Error(string message, ErrorType type, string code = GeneralErrorCode, ValidationErrors? validationErrors = null)
    {
        Message = message;
        Type = type;
        Code = code;

        if (validationErrors is not null)
            ValidationErrors = validationErrors;
    }

    public static Error Failure(
        string message = "Operation failed",
        string code = "Error.General.Failure")
        => new(message, ErrorType.Failure, code);
    public static Error Unexpected(
        string message = "Unexpected error has occurred",
        string code = "Error.General.Unexpected")
        => new(message, ErrorType.Unexpected, code);
    public static Error NotFound(
        string message = "Item not found",
        string code = "Error.General.NotFound")
        => new(message, ErrorType.NotFound, code);
    public static Error Unauthorized(
        string message = "Item not found",
        string code = "Error.General.Unauthorized")
        => new(message, ErrorType.Unauthorized, code);
    public static Error Forbidden(
        string message = "Forbidden",
        string code = "Error.General.Forbidden")
        => new(message, ErrorType.Forbidden, code);
    public static Error Validation(ValidationError validationError)
        => new(validationError.Message, ErrorType.Validation, validationError.Code);
    public static Error Validation(
        ValidationErrors validationErrors,
        string message = "One or more validation error has occurred",
        string code = "Error.General.Validation")
        => new(message, ErrorType.Validation, code, validationErrors);
    public static Error Validation<T>(Value<T> value, ValidationError validationError)
    {
        var errors = new ValidationErrors
        {
            { value, validationError }
        };

        return Validation(errors);
    }
}
