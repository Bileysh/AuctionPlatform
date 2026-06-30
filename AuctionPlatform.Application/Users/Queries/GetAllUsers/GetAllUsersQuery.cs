using AuctionPlatform.Application.Users.Queries.GetUserById;
using MediatR;

namespace AuctionPlatform.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(): IRequest<List<UserDto>>;