using MediatR;

namespace AuctionPlatform.Application.Users.Commands.UpdateUser;

public record UpdateUserCommand(string Name) : IRequest<bool>;