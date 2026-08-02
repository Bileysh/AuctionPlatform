using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Queries.GetCurrentUser;

public class GetCurrentUserQueryHandler: IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public GetCurrentUserQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        
        if (string.IsNullOrEmpty(auth0Id))
            throw new UnauthorizedAccessException();

        var user = await _context.Users
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);

        if (user == null)
            throw new NotFoundException("User", auth0Id);

        return new UserDto(
            user.Id,
            user.UserName,
            user.Auth0Id,
            user.Balance,
            user.GetAvailableBalance()
        );
    }
}