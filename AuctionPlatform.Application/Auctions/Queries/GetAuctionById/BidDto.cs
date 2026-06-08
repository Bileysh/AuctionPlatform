namespace AuctionPlatform.Application.Auctions.Queries.GetAuctionById;

public record BidDto(Guid Id, string BidderName, decimal Amount, DateTime CreatedAt);