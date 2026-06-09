using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Transactions.Queries.GetAllTransaction;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Transactions.Queries.GetTransactionById;

public class GetTransactionsByIdQueryHandler: IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    private readonly IApplicationDbContext _context;
    
    public GetTransactionsByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var query = await _context.Transactions
            .AsNoTracking()
            .Select(t => new TransactionDto(
                t.Id, t.UserId, t.User.UserName, t.Amount, t.Type.ToString(), t.ReferenceId, t.CreatedAt))
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (query == null) 
            throw new NotFoundException(nameof(Transaction), request.Id);
        
        return query;
    }
}