using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Commands.Deposit;

public class DepositCommandHandler: IRequestHandler<DepositCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public DepositCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        
        if (string.IsNullOrEmpty(auth0Id))
            throw new UnauthorizedAccessException("You must be logged in to make a deposit.");
        
        var user = await _context.Users
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);
        
        if (user == null)
            throw new NotFoundException(nameof(User), auth0Id);
        
        user.Deposit(request.Amount);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}