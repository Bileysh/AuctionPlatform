using MediatR;

namespace AuctionPlatform.Application.Users.Commands.CreateUser;

public record CreateUserCommand(string Username) : IRequest<Guid>;