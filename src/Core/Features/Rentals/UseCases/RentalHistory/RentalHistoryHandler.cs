using Core.Domain.Common;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Rentals.UseCases.RentalHistory;
public class RentalHistoryHandler(AppDbContext appDbContext)
    : BaseHandler<RentalHistoryRequest, RentalHistoryResponse>
{
    protected override async Task<Result<RentalHistoryResponse>> Handle(
        RentalHistoryRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

        if (user == null)
            return Error.NotFound("Current user information is not found");


        var rentals = await appDbContext.Rentals
            .Include(r => r.RentalItems)
                .ThenInclude(ri => ri.Movie)
            .Where(r => r.userid == user.Id &&
                        !r.DeletionInfo.IsDeleted)
            .OrderByDescending(r => r.RentDate)
            .ToListAsync(cancellationToken);


        var rentalHistory = rentals.Select(r => new RentalHistoryItem(
            r.Id,
            r.RentDate,
            r.ReturnDate,
            r.TotalCost,
            r.Status,
            r.RentalItems.Select(ri => ri.Movie.Title).ToList()
        )).ToList();


        decimal totalAmountSpent = rentals.Sum(r => r.TotalCost);

        return new RentalHistoryResponse(
            user.Id,
            user.PersonalName.FullName,
            user.Email.EmailNormalized,
            user.LoyaltyPoints,
            totalAmountSpent,
            rentalHistory
        );
    }
}
