namespace Core.Features.Rentals.UseCases.MovieRental;
public class MovieRentalRequest
{ 
    public List<long> MovieIds { get; set; } = new();
    public int RentalDays { get; set; }
}
