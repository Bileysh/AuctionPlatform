using AuctionPlatform.Domain.Entities.Enums;

namespace AuctionPlatform.Application.Auctions.Queries.GetAuctionById;

public record AuctionDetailsDto(Guid Id,
    string Title,
    string Description,
    decimal CurrentPrice,
    DateTime EndsAt,
    string CategoryName,
    string SellerName,
    AuctionStatus Status,
    List<BidDto> Bids,
    List<CommentDto> Comments);