using AuctionPlatform.Application.Common.Interfaces;
using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.DeleteComment;

public record DeleteCommentCommand(Guid Id) : IRequest<bool>, IOwnedResourceRequest
{
    public Guid ResourceId => Id;
    public ResourceType Type => ResourceType.Comment;
}