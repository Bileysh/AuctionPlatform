using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
using AuctionPlatform.Application.Common.Models;
using MediatR;

namespace AuctionPlatform.Application.Bids.Queries.GetMyBiddedAuctions;

public record GetMyBiddedAuctionsQuery : IRequest<PaginatedList<BiddedAuctionDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}