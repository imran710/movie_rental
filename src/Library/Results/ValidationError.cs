namespace Library.Results;

public readonly record struct ValidationError
{
    public readonly string Message { get; init; }
    public readonly string Code { get; init; }

    public ValidationError(string message, string code)
    {
        Message = message;
        Code = code;
    }

    public static ValidationError Create(string message, string code) => new(message, code);
}
