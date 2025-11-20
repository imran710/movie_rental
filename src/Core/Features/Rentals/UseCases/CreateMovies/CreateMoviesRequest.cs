using Core.Features.Rentals.Enums;

namespace Core.Features.Rentals.UseCases.CreateMovies;
public record CreateMoviesRequest
(
    string Title,
    string Genre,
    int ReleaseYear,
    int Stock,
    RentalType RentalType
);
