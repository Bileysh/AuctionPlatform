namespace AuctionPlatform.Application.Bids.Queries.GetMyBiddedAuctions;

public record BiddedAuctionDto(
    Guid Id, 
    string Title, 
    decimal CurrentPrice, 
    DateTime EndsAt, 
    string CategoryName,
    decimal MyHighestBid 
);