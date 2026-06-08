using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.UpdateAuction;

public record UpdateAuctionCommand(Guid Id, string Title, string? Description, DateTime EndsAt, int CategoryId) : IRequest<bool>;