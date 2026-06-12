using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.AddComment;

public record AddCommentCommand(Guid AuctionId, string Text) : IRequest<Guid>;