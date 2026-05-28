namespace AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;

public record AuctionDto(
    Guid Id,
    string Title,
    decimal CurrentPrice,
    DateTime EndsAt,
    string CategoryName);