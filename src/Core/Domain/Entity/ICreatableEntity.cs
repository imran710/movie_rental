using Core.Domain.Audit;

namespace Core.Domain.Entity;

public interface ICreatableEntity
{
    public CreationInfo CreationInfo { get; }
}

public interface ICreatableEntity<T> where T : struct
{
    public CreationInfo<T> CreationInfo { get; }
}
