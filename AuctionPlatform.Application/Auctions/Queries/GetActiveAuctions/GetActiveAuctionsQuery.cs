using MediatR;

namespace AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;

public record GetActiveAuctionsQuery : IRequest<List<AuctionDto>>;