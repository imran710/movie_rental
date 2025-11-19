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

    public ICollection<RentalItem> RentalItems { get; set; }= new List<RentalItem>();

    public  UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated; 
    public required CreationInfo CreationInfo { get; init; }
    public DeletionInfo DeletionInfo { get; } = DeletionInfo.NoDeleted();

    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }
    public bool IsAvailable()
    {
        return Stock > 0 && !DeletionInfo.IsDeleted;
    }
    public void Update()
    {
        if (UpdateInfo.UpdatedBy == null)
            throw new InvalidOperationException("UpdatedBy must be set before updating.");

        UpdateInfo.MarkAsUpdated(UpdateInfo.UpdatedBy.Value);
    }
    public void DecreaseStock()
    {
        if (Stock <= 0)
            throw new InvalidOperationException($"Movie '{Title}' is out of stock");

        Stock--;
        Update();
    }

    public void IncreaseStock()
    {
        Stock++;
        Update();
    }
    public static Movie Create(
    string title,
    string genre,
    int releaseYear,
    int stock,
    RentalType rentalType,
    long createdBy)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Movie title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("Genre is required.", nameof(genre));

        if (releaseYear <= 1900 || releaseYear > DateTime.UtcNow.Year)
            throw new ArgumentException("Invalid release year.", nameof(releaseYear));

        if (stock < 0)
            throw new ArgumentException("Stock cannot be negative.", nameof(stock));

        var movie = new Movie
        {
            Title = title.Trim(),
            Genre = genre.Trim(),
            ReleaseYear = releaseYear,
            Stock = stock,
            RentalType = rentalType,

            CreationInfo = new CreationInfo(createdBy)
        };

        return movie;
    }

}
