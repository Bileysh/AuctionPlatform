using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
using AuctionPlatform.Application.Common.Models;
using MediatR;

namespace AuctionPlatform.Application.Auctions.Queries.GetMyAuctions;

public record GetMyAuctionsQuery : IRequest<PaginatedList<AuctionDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}