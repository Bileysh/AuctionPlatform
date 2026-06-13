using AuctionPlatform.Application.Common.Models;
using MediatR;

namespace AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;

public record GetActiveAuctionsQuery : IRequest<PaginatedList<AuctionDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;

    public string? SearchTerm { get; init; }
    public int? CategoryId { get; init; }
    
}