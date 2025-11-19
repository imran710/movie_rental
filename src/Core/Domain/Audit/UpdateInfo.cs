using Core.Domain.Helper;

namespace Core.Domain.Audit;

public record UpdateInfo<T> where T : struct
{
    public T? UpdatedBy { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public bool IsUpdated => UpdatedAt.HasValue;

    protected UpdateInfo() { }

    protected UpdateInfo(T? updatedBy, DateTimeOffset? updatedAt)
    {
        UpdatedBy = updatedBy;
        UpdatedAt = updatedAt;
    }

    public void MarkAsUpdated(T updatedBy) =>
        (UpdatedBy, UpdatedAt) = (updatedBy, DateTimeHelperStatic.Now);

    public bool IsEqual(UpdateInfo<T> updateInfo) => Nullable.Equals(updateInfo.UpdatedBy, UpdatedBy);

    public override string ToString()
    {
        return IsUpdated
            ? $"Last updated by UserId: {UpdatedBy} at {UpdatedAt?.ToString("u")}"
            : "Not Updated";
    }
}

public sealed record UpdateInfo : UpdateInfo<long>
{
    public static UpdateInfo NotUpdated => new();
}
