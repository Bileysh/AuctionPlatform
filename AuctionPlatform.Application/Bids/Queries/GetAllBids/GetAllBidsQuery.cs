using MediatR;

namespace AuctionPlatform.Application.Bids.Queries.GetAllBids;

public record GetAllBidsQuery() : IRequest<List<BidDto>>;