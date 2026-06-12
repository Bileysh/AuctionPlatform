using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.PlaceBid;

public record PlaceBidCommand(
    Guid AuctionId, 
    decimal Amount) : IRequest<bool>;