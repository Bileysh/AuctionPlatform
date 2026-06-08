namespace AuctionPlatform.Application.Bids.Queries.GetAllBids;

public record BidDto(
    Guid Id, 
    Guid AuctionId, 
    string BidderName, 
    decimal Amount, 
    DateTime CreatedAt
);