using Core.Domain.Helper;

namespace Core.Domain.Audit;

public record DeletionInfo<T> where T : struct
{
    public bool IsDeleted { get; private set; } = false;
    public T? DeletedBy { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public void MarkAsDeleted(T deletedBy) => (IsDeleted, DeletedBy, DeletedAt) =
        (true, deletedBy, DateTimeHelperStatic.Now);

    public void MarkAsDeleted() => (IsDeleted, DeletedBy, DeletedAt) =
        (true, default, DateTimeHelperStatic.Now);

    public void Restore() => (IsDeleted, DeletedBy, DeletedAt) =
        (false, default, default);

    public bool IsRestorable => IsDeleted;

    public bool IsEqual(DeletionInfo<T> deletionInfo)
        => deletionInfo.IsDeleted == IsDeleted && Nullable.Equals(deletionInfo.DeletedBy, DeletedBy);

    public override string ToString()
    {
        return IsDeleted
            ? $"Deleted by UserId: {DeletedBy} at {DeletedAt?.ToString("u")}"
            : "Not Deleted";
    }
}

public sealed record DeletionInfo : DeletionInfo<long>
{
    public static DeletionInfo NoDeleted() => new();
}
