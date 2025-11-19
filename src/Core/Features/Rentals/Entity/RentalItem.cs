using Core.Domain.Audit;
using Core.Domain.Entity;

namespace Core.Features.Rentals.Entity;
public class RentalItem : IEntity, IUpdatableEntity, IDeletableEntity
{
    public long Id { get; init; }
    public int RentalId { get; set; }
    public RentalE Rental { get; set; } = default!;

    public int MovieId { get; set; }
    public Movie Movie { get; set; }= default!;

    public UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated;
    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();
}
