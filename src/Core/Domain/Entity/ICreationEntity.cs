using Core.Domain.Audit;

namespace Core.Domain.Entity;

public interface ICreationEntity
{
    public CreationTime CreationTime { get; }
}
