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
    private readonly ICurrentUserService _currentUserService;
    
    public GetTransactionsByIdQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);

        if (currentUser == null)
             throw new NotFoundException(nameof(User), auth0Id!);
        
        var query = await _context.Transactions
            .AsNoTracking()
            .Select(t => new TransactionDto(
                t.Id, t.UserId, t.User.UserName, t.Amount, t.Type.ToString(), t.ReferenceId, t.CreatedAt))
            .FirstOrDefaultAsync(t => t.Id == request.Id && t.UserId == currentUser.Id, cancellationToken);
        
        if (query == null) 
            throw new NotFoundException(nameof(Transaction), request.Id);
        
        return query;
    }
}