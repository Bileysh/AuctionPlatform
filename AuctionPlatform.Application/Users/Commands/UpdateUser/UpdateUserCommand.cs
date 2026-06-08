using MediatR;

namespace AuctionPlatform.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(Guid Id, string Name) : IRequest<bool>;