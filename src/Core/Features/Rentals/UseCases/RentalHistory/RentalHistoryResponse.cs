using Core.Features.Rentals.Enums;

namespace Core.Features.Rentals.UseCases.RentalHistory;
public record RentalHistoryResponse(
   long UserId,
    string UserName,
    string Email,
    int LoyaltyPoints,
    decimal TotalAmountSpent,
    List<RentalHistoryItem> Rentals
);
public record RentalHistoryItem(
    long RentalId,
    DateTime RentDate,
    DateTime? ReturnDate,
    decimal TotalCost,
    RentalStatus Status,
    List<string> MovieTitles
);
