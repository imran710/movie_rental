namespace Core.Domain.Entity;

public interface IAuditableEntity : IEntity, ICreatableEntity, IUpdatableEntity, IDeletableEntity
{
}

public interface IAuditableEntity<T> : IEntity<T>, ICreatableEntity<T>, IUpdatableEntity<T>, IDeletableEntity<T> where T : struct
{
}
