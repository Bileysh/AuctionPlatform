using AuctionPlatform.Application.Common.Interfaces;
using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.CancelAuction;

public record CancelAuctionCommand(Guid Id) : IRequest<bool>, IOwnedResourceRequest
{
    public Guid ResourceId => Id;
    public ResourceType Type => ResourceType.Auction;
}