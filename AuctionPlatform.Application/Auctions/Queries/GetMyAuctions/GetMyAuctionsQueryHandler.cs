using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Queries.GetMyAuctions;

public class GetMyAuctionsQueryHandler : IRequestHandler<GetMyAuctionsQuery, PaginatedList<AuctionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public GetMyAuctionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<PaginatedList<AuctionDto>> Handle(GetMyAuctionsQuery request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);

        if (user == null)
        {
            return new PaginatedList<AuctionDto>(new List<AuctionDto>(), 0, request.PageNumber, request.PageSize);
        }

        var userGuid = user.Id; 

        var query = _context.AuctionItems
            .AsNoTracking()
            .Where(a => a.SellerId == userGuid)
            .OrderByDescending(a => a.CreatedAt);
        
        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize) 
            .Take(request.PageSize)                            
            .Select(a => new AuctionDto(
                a.Id,
                a.Title,
                a.CurrentPrice,
                a.EndsAt,
                a.Category.Name
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedList<AuctionDto>(items, totalCount, request.PageNumber, request.PageSize);
    }
}