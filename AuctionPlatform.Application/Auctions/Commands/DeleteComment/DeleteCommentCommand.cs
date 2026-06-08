using MediatR;

namespace AuctionPlatform.Application.Auctions.Commands.DeleteComment;

public record DeleteCommentCommand(Guid Id) : IRequest<bool>;