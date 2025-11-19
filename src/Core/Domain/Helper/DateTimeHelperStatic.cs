namespace Core.Domain.Helper;

public static class DateTimeHelperStatic
{
    public static DateTimeOffset Now => DateTimeOffset.UtcNow;
    public static DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}