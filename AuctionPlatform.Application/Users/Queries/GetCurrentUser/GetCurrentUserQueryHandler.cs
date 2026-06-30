using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Users.Queries.GetUserById;
using AuctionPlatform.Domain.Entities.Enums;
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

        var userDto = await _context.Users
            .Where(u => u.Auth0Id == auth0Id)
            .Select(u => new UserDto(
                u.Id,
                u.UserName,
                u.Auth0Id,
                u.Balance,
                u.Balance - (
                    u.Transactions.Where(t => t.Type == TransactionType.Hold).Sum(t => t.Amount) - 
                    u.Transactions.Where(t => t.Type == TransactionType.Release || t.Type == TransactionType.Payment).Sum(t => t.Amount)
                )
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (userDto == null)
            throw new NotFoundException("User", auth0Id);

        return userDto;
    }
}