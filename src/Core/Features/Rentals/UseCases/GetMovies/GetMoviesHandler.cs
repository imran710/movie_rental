using Core.Domain.Common;
using Core.Features.Rentals.Entity;
using Core.Infrastructure.Database;
using Core.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Core.Features.Rentals.UseCases.GetMovies;
public class GetMoviesHandler(AppDbContext appDbContext)
    : BaseHandler<GetMoviesRequest, GetMoviesResponse>
{
    protected override async Task<Result<GetMoviesResponse>> Handle(
        GetMoviesRequest request,
        CancellationToken cancellationToken = default)
    {
        var currentUser = _httpContext.GetCurrentUser();

        var user = await appDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == currentUser.UserId, cancellationToken);

        if (user == null)
            return Error.NotFound("Current user information is not found");
         
        IQueryable<Movie> query = appDbContext.Movies
            .Where(m => m.Stock > 0 && !m.DeletionInfo.IsDeleted);


        var movies = await query.ToListAsync(cancellationToken);

        if (movies.Count == 0)
            return Error.NotFound("No available movies found");

        return new GetMoviesResponse(movies);
    }
}