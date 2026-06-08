using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;

public class GetAllTransactionsQueryHandler : IRequestHandler<GetAllTransactionsQuery, List<TransactionDto>>
{
    private readonly IApplicationDbContext _context;
    
    public GetAllTransactionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<TransactionDto>> Handle(GetAllTransactionsQuery request, CancellationToken cancellationToken)
    {
        var query = await _context.Transactions
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TransactionDto(
                t.Id, t.UserId, t.User.UserName, t.Amount, t.Type.ToString(), t.ReferenceId, t.CreatedAt))
            .ToListAsync(cancellationToken);

        return query;
    }
}