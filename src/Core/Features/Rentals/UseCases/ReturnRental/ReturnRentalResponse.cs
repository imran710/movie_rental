namespace Core.Features.Rentals.UseCases.ReturnRental;
public record ReturnRentalResponse(
 long RentalId, decimal FinalAmount, int EarnedPoints);
