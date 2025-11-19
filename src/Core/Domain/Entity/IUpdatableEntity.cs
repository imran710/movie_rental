using Core.Domain.Audit;

namespace Core.Domain.Entity;

public interface IUpdatableEntity
{
    public UpdateInfo UpdateInfo { get; }
}

public interface IUpdatableEntity<T> where T : struct
{
    public UpdateInfo<T> UpdateInfo { get; }
}
