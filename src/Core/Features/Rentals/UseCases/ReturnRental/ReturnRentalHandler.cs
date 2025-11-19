using Core.Domain.Common;
using Core.Features.Rentals.Enums;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Rentals.UseCases.ReturnRental;
public class ReturnRentalHandler(AppDbContext appDbContext)
    : BaseHandler<ReturnRentalRequest, ReturnRentalResponse>
{
    protected override async Task<Result<ReturnRentalResponse>> Handle(
        ReturnRentalRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

        if (user == null)
            return Error.NotFound("Current user information is not found");

        var rental = await appDbContext.Rentals
            .Include(r => r.RentalItems)
                .ThenInclude(ri => ri.Movie)
            .FirstOrDefaultAsync(r => r.Id == request.RentalId, cancellationToken);

        if (rental == null)
            return Error.NotFound("Rental record not found");

        if (rental.Status == RentalStatus.Returned)
            return Error.Failure("Rental already returned");

        if (rental.userid != user.Id)
            return Error.Forbidden("You cannot return a rental that doesn't belong to you");
         
        foreach (var item in rental.RentalItems)
        {
            item.Movie.UpdateInfo.MarkAsUpdated(user.Id);
            item.Movie.IncreaseStock();
        } 
        var returnedAt = DateTime.UtcNow;
        decimal totalAmount = 0m;

        foreach (var item in rental.RentalItems)
        {
            var movie = item.Movie;
             
            var dailyRate = movie.RentalType switch
            {
                RentalType.NewRelease => 5m,
                RentalType.Regular => 3m,
                RentalType.Classic => 2m,
                _ => 3m
            };
             
            var actualDays = (int)Math.Ceiling((returnedAt - rental.RentDate).TotalDays);
             
            var chargeableDays = Math.Max(3, actualDays);
            totalAmount += chargeableDays * dailyRate;
             
            var lateDays = Math.Max(0, actualDays - rental.RentalDays);
            if (lateDays > 0)
            {
                var lateFee = lateDays * (dailyRate * 1.5m);
                totalAmount += lateFee;
            }
        } 
        if (user.LoyaltyPoints >= 100)
            totalAmount = Math.Round(totalAmount * 0.9m, 2); // 10% discount

        // Earn 1 loyalty point per $1 spent
        var earnedPoints = (int)Math.Floor(totalAmount);
        user.LoyaltyPoints += earnedPoints; 
        rental.Status = RentalStatus.Returned;
        rental.ReturnDate = returnedAt;
        rental.TotalCost = totalAmount;

        await appDbContext.SaveChangesAsync(cancellationToken);

        return new ReturnRentalResponse(
            rental.Id,
            totalAmount,
            earnedPoints
        );
    }
}
