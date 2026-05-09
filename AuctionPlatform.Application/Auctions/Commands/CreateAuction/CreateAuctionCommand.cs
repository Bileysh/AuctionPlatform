using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.CreateAuction;

public record CreateAuctionCommand(
    string Title, 
    string Description, 
    decimal StartingPrice, 
    DateTime EndsAt, 
    Guid SellerId) : IRequest<Guid>;