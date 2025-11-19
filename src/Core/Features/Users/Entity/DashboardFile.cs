using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Infrastructure.Common.Enums;

namespace Core.Features.Users.Entity;
public class DashboardFile : IEntity, IDeletableEntity, ICreatableEntity
{
    public long Id { get; }
    public required long UserId { get; init; }
    public User? User { get; init; }
    public required string FileUrl { get; set; }
    public DashboardFileType FileType { get; set; }
    public required CreationInfo CreationInfo { get; init; }
    public DeletionInfo DeletionInfo { get; } = DeletionInfo.NoDeleted();

    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }
    

}