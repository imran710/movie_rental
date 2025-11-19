namespace Core.Domain.Helper;

public interface IDateTimeHelper
{
    DateTimeOffset Now { get; }
    DateTimeOffset UtcNow { get; }
}