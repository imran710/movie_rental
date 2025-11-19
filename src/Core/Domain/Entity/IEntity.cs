namespace Core.Domain.Entity;

public interface IEntity
{
    long Id { get; }
}

public interface IEntity<out T> where T : struct
{
    T Id { get; }
}
