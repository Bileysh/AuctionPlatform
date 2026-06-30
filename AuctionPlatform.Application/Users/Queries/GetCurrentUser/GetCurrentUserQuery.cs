using AuctionPlatform.Application.Users.Queries.GetUserById;
using MediatR;

namespace AuctionPlatform.Application.Users.Queries.GetCurrentUser;

public class GetCurrentUserQuery : IRequest<UserDto>
{
}