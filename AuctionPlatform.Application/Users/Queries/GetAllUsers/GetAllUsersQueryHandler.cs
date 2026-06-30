using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler: IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly IApplicationDbContext _context;
    
    public GetAllUsersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.Users
            .Include(u => u.Transactions)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
     
        return users.Select(user => new UserDto(
            user.Id,
            user.UserName,
            user.Auth0Id,
            user.Balance,
            user.GetAvailableBalance()
            )).ToList();
    }
}