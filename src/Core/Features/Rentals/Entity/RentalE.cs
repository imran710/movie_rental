using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.Rentals.Enums;
using Core.Features.Users.Entity;

namespace Core.Features.Rentals.Entity;
public class RentalE : IEntity, IUpdatableEntity, IDeletableEntity
{
    public long Id { get; init; }
    public int userid { get; set; }
    public User Customer { get; set; } = null!;

    public DateTime RentDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal TotalCost { get; set; }
    public RentalStatus Status { get; set; }

    public ICollection<RentalItem> RentalItems { get; set; } = new List<RentalItem>();

    public UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated;
    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();
}
