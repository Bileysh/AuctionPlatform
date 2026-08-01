using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public GetUserByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Transactions)
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);
        
        if (user == null)
            throw new NotFoundException(nameof(User), request.Id);

        bool isOwner = !string.IsNullOrEmpty(_currentUserService.Auth0Id) && 
                       _currentUserService.Auth0Id == user.Auth0Id;

        if (isOwner)
        {
            return new UserDto(
                user.Id,
                user.UserName,
                user.Auth0Id,
                user.Balance,
                user.GetAvailableBalance()
            );
        }
        
        return new UserDto(
            user.Id,
            user.UserName
        );
    }
}