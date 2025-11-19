using Core.Domain.Common;
using Core.Features.Rentals.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Rentals.UseCases.MovieRental;
public class MovieRentalHandler(AppDbContext appDbContext)
    : BaseHandler<MovieRentalRequest, MovieRentalResponse>
{
    protected override async Task<Result<MovieRentalResponse>> Handle(
        MovieRentalRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
            return Error.NotFound("Current user information is not found");

         
        var movies = await appDbContext.Movies
            .Where(m => request.MovieIds.Contains(m.Id))
            .ToListAsync(cancellationToken);

        if (movies.Count == 0)
            return Error.NotFound("No valid movies found for rental"); 
        var rental = RentalE.Create(
            customer: user,
            movies: movies,
            rentalDays: request.RentalDays,
            createdBy: currentUser.UserId
        );
         
        appDbContext.Rentals.Add(rental);
        await appDbContext.SaveChangesAsync(cancellationToken); 
        return new MovieRentalResponse(rental.Id);
    }
}
