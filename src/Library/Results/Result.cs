namespace Library.Results;

public readonly record struct Result<T>
{
    private readonly T _value = default!;
    public T Value => IsError ? throw new ResultInvalidOperationException("Can not access value when result is an error") : _value;
    public readonly Error Error { get; init; } = Error.Unexpected("Unable to retrieve error from result");
    public bool IsError { get; init; }

    // Constructors
    public Result(T value)
    {
        _value = value;
    }

    public Result(Error error)
    {
        Error = error;
        IsError = true;
    }

    // Implicit operators
    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }

    public TResult Match<TResult>(Func<T, TResult> onValue, Func<Error, TResult> onError)
    {
        if (IsError)
        {
            return onError(Error);
        }

        return onValue(Value);
    }
}

public static class Result
{
    public static Result<T> Success<T>(T value) => new(value);
    public static Result<T> Error<T>(Error error) => new(error);
}
