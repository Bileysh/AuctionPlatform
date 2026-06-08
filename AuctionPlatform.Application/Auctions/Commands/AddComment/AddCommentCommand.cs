using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.AddComment;

public record AddCommentCommand(Guid AuctionId, Guid AuthorId, string Text) : IRequest<Guid>;