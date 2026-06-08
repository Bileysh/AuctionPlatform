using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.CancelAuction;

public record CancelAuctionCommand(Guid Id) : IRequest<bool>;