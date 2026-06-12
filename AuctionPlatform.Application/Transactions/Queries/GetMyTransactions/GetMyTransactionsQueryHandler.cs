using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Transactions.Queries.GetMyTransactions;

public class GetMyTransactionsQueryHandler : IRequestHandler<GetMyTransactionsQuery, List<TransactionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public GetMyTransactionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<TransactionDto>> Handle(GetMyTransactionsQuery request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);
        
        if (currentUser == null)
            throw new UnauthorizedAccessException("You must be logged in to view transactions.");

        
        var query = await _context.Transactions
            .AsNoTracking()
            .Where(t => t.UserId == currentUser.Id) 
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto(
                t.Id, t.UserId, t.User.UserName, t.Amount, t.Type.ToString(), t.ReferenceId, t.CreatedAt))
            .ToListAsync(cancellationToken);

        return query;
    }
}