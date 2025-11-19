namespace Core.Domain.Helper;

public class DateTimeHelper : IDateTimeHelper
{
    public DateTimeOffset Now => DateTimeHelperStatic.Now;
    public DateTimeOffset UtcNow => DateTimeHelperStatic.UtcNow;
}