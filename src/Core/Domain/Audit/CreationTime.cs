using Core.Domain.Helper;

namespace Core.Domain.Audit;

public record CreationTime
{
    public DateTimeOffset CreatedAt { get; } = DateTimeHelperStatic.Now;

    public static CreationTime Create() => new();
}
