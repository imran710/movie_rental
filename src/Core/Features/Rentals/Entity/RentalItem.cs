using Core.Domain.Audit;
using Core.Domain.Entity;

namespace Core.Features.Rentals.Entity;
public class RentalItem : IEntity, IUpdatableEntity, IDeletableEntity
{
    public long Id { get; init; }
    public int RentalId { get; set; }
    public RentalE Rental { get; set; } = default!;

    public long MovieId { get; set; }
    public Movie Movie { get; set; }= default!;

    public UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated;
    public required CreationInfo CreationInfo { get; init; }
    public DeletionInfo DeletionInfo { get; } = DeletionInfo.NoDeleted();

    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }
}
