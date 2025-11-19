using Core.Domain.Audit;

namespace Core.Domain.Entity;

public interface IDeletableEntity
{
    public DeletionInfo DeletionInfo { get; }
}

public interface IDeletableEntity<T> where T : struct
{
    public DeletionInfo<T> DeletionInfo { get; }
}
