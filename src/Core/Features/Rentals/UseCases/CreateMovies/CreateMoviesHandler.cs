using Core.Domain.Common;
using Core.Features.Rentals.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Rentals.UseCases.CreateMovies;
public class CreateMoviesHandler(AppDbContext appDbContext)
    : BaseHandler<CreateMoviesRequest, CreateMoviesResponse>
{
    protected override async Task<Result<CreateMoviesResponse>> Handle(
        CreateMoviesRequest request,
        CancellationToken cancellationToken = default)
    {

        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken)
            .ConfigureAwait(false);

        if (user == null)
            return Error.NotFound("Current user information is not found"); 

        var movie = Movie.Create(
            title: request.Title,
            genre: request.Genre,
            releaseYear: request.ReleaseYear,
            stock: request.Stock,
            rentalType: request.RentalType,
            createdBy: currentUser.UserId
        ); 
        appDbContext.Movies.Add(movie);
        await appDbContext.SaveChangesAsync(cancellationToken); 
        return new CreateMoviesResponse(movie.Id);
    }
}

