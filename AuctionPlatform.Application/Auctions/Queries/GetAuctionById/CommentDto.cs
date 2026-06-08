namespace AuctionPlatform.Application.Auctions.Queries.GetAuctionById;

public record CommentDto(Guid Id, string AuthorName, string Text, DateTime CreatedAt);