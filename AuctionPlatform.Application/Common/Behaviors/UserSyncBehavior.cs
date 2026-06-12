using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Common.Behaviors;

public class UserSyncBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IApplicationDbContext _context;
    
    public UserSyncBehavior(ICurrentUserService currentUserService, IApplicationDbContext context)
    {
        _currentUserService = currentUserService;
        _context = context;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_currentUserService.IsAuthenticated)
            return await next();
        
        var auth0Id = _currentUserService.Auth0Id;
        
        var userExists = await _context.Users
            .AnyAsync(u => u.Auth0Id == auth0Id, cancellationToken);
        
        if(!userExists)
        {
            var newUser = new User(auth0Id!, _currentUserService.UserName!);
            
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync(cancellationToken);
        }
        
        return await next();
    }
}