using MediatR;

namespace AuctionPlatform.WebApi.DTO;

public record AddCommentRequest(Guid AuthorId, string Text);