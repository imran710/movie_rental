using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.Rentals.Enums;

namespace Core.Features.Rentals.Entity;
public class Movie : IEntity, IUpdatableEntity, IDeletableEntity
{
    public long Id { get; init; }
    public string Title { get; set; }=string.Empty;
    public string Genre { get; set; } = string.Empty;
    public int ReleaseYear { get; set; }
    public int Stock { get; set; }
    public RentalType RentalType { get; set; }

    public UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated;
    public DeletionInfo DeletionInfo { get; private set; } = DeletionInfo.NoDeleted();
    public ICollection<RentalItem> RentalItems { get; set; }= new List<RentalItem>();
}
