using AuctionPlatform.Application.Auctions.Queries.GetActiveAuctions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Application.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Bids.Queries.GetMyBiddedAuctions;

public class GetMyBiddedAuctionsQueryHandler : IRequestHandler<GetMyBiddedAuctionsQuery, PaginatedList<BiddedAuctionDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyBiddedAuctionsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<PaginatedList<BiddedAuctionDto>> Handle(GetMyBiddedAuctionsQuery request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;

        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);

        if (user == null)
        {
            return new PaginatedList<BiddedAuctionDto>(new List<BiddedAuctionDto>(), 0, request.PageNumber, request.PageSize);
        }

        var userId = user.Id;

        var query = _context.AuctionItems
            .AsNoTracking()
            .Where(a => a.Bids.Any(b => b.BidderId == userId))
            .OrderByDescending(a => a.EndsAt); 

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(a => new BiddedAuctionDto(
                a.Id,
                a.Title,
                a.CurrentPrice,
                a.EndsAt,
                a.Category.Name,
                a.Bids.Where(b => b.BidderId == userId).Max(b => b.Amount) 
            ))
            .ToListAsync(cancellationToken);

        return new PaginatedList<BiddedAuctionDto>(items, totalCount, request.PageNumber, request.PageSize);    }
}