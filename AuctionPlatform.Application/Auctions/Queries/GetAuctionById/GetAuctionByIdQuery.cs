using MediatR;

namespace AuctionPlatform.Application.Auctions.Queries.GetAuctionById;

public record GetAuctionByIdQuery(Guid Id) : IRequest<AuctionDetailsDto>;