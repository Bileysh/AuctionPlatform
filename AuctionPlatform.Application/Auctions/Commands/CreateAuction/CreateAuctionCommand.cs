using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.CreateAuction;

public record CreateAuctionCommand(
    string Title, 
    string Description, 
    decimal StartingPrice, 
    DateTime EndsAt, 
    int CategoryId) : IRequest<Guid>;