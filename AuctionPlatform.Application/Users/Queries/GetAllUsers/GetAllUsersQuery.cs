using MediatR;

namespace AuctionPlatform.Application.Users.Queries.GetAllUsers;

public record GetAllUsersQuery(): IRequest<List<UserDto>>;