namespace Core.Domain.Exceptions;

public class DomainException(Error error) : Exception
{
    public Error Error { get; init; } = error;

    public static DomainException Create(ValidationError validationError) => new(Error.Validation(validationError));
    public static DomainException Create(Error error) => new(error);
}
