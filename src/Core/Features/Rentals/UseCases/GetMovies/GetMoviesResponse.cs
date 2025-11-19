using Core.Features.Rentals.Entity;
using Core.Features.Rentals.Enums;

namespace Core.Features.Rentals.UseCases.GetMovies;
public record GetMoviesResponse(List<Movie> Movies);