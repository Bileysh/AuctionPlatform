using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Users.Commands.Deposit;

public class DepositCommandHandler: IRequestHandler<DepositCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public DepositCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DepositCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        
        if (user == null)
            throw new NotFoundException(nameof(User), request.UserId);
        
        user.Deposit(request.Amount);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return true;
    }
}