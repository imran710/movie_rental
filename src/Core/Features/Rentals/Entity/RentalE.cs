using Core.Domain.Audit;
using Core.Domain.Entity;
using Core.Features.Rentals.Enums;
using Core.Features.Users.Entity;

namespace Core.Features.Rentals.Entity;
public class RentalE : IEntity, IUpdatableEntity, IDeletableEntity
{
    public long Id { get; init; }
    public long userid { get; set; }
    public User Customer { get; set; } = null!;
    public int RentalDays { get; set; }          
    public DateTime RentDate { get; set; }
    public DateTime? ReturnDate { get; set; }
    public decimal TotalCost { get; set; }
    public RentalStatus Status { get; set; }

    public ICollection<RentalItem> RentalItems { get; set; } = new List<RentalItem>();
    public UpdateInfo UpdateInfo { get; private set; } = UpdateInfo.NotUpdated;
    public required CreationInfo CreationInfo { get; init; }
    public DeletionInfo DeletionInfo { get; } = DeletionInfo.NoDeleted();

    public void MarkAsDeleted()
    {
        DeletionInfo.MarkAsDeleted();
    }

    public static RentalE Create(User customer, List<Movie> movies, int rentalDays, long createdBy)
    {
        if (customer is null)
            throw new ArgumentNullException(nameof(customer));

        if (movies == null || movies.Count == 0)
            throw new ArgumentException("At least one movie must be selected.", nameof(movies));

        if (rentalDays <= 0)
            throw new ArgumentException("Rental days must be positive.", nameof(rentalDays));
         
        foreach (var movie in movies)
        {
            movie.DecreaseStock();   
        }

        var rental = new RentalE
        {
            userid = customer.Id,
            Customer = customer,
            RentDate = DateTime.UtcNow,
            Status = RentalStatus.Active,

            CreationInfo = new CreationInfo(createdBy),

            RentalItems = movies.Select(m => new RentalItem
            {
                MovieId = m.Id,
                CreationInfo = new CreationInfo(createdBy)
            }).ToList()
        };

        rental.TotalCost = CalculateTotalCost(movies, rentalDays);

        return rental;
    }




    private static decimal CalculateTotalCost(List<Movie> movies, int rentalDays)
    {
        decimal total = 0;
        foreach (var movie in movies)
        { 
            total += CalculateMovieRentalCost(movie.RentalType, rentalDays);
        }
        return total;
    }

    private static decimal CalculateMovieRentalCost(RentalType rentalType, int days)
    { 
        decimal dailyRate = rentalType switch
        {
            RentalType.NewRelease => 5.00m,
            RentalType.Regular => 3.00m,
            RentalType.Classic => 2.00m,
            _ => 3.00m
        }; 
        var chargeableDays = Math.Max(days, 3);
        return dailyRate * chargeableDays;
    }
}
