using AuctionPlatform.Application.Bids.Queries.GetAllBids;
using MediatR;

namespace AuctionPlatform.Application.Bids.Queries.GetBidById;

public record GetBidByIdQuery(Guid Id) : IRequest<BidDto>;