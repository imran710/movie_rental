using Core.Domain.Helper;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Domain.Audit;
[ComplexType]
public abstract record CreationInfo<T>(T CreatedBy)
    where T : struct
{
    public DateTimeOffset CreatedAt { get; } = DateTimeHelperStatic.Now;

    public bool IsEqual(CreationInfo<T> creationInfo) => Nullable.Equals<T>(creationInfo.CreatedBy, CreatedBy);

    public override string ToString() => $"Created by UserId: {CreatedBy} at {CreatedAt:u}";
}

public sealed record CreationInfo(long CreatedBy) : CreationInfo<long>(CreatedBy);
