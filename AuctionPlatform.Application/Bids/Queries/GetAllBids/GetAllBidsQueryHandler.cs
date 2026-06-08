using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Bids.Queries.GetAllBids;

public class GetAllBidsQueryHandler: IRequestHandler<GetAllBidsQuery, List<BidDto>>
{
    private readonly IApplicationDbContext _context;
    
    public GetAllBidsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<BidDto>> Handle(GetAllBidsQuery request, CancellationToken cancellationToken)
    {
        var bids = await _context.Bids
            .AsNoTracking()
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new BidDto(b.Id, b.AuctionItemId, b.Bidder.UserName, b.Amount, b.CreatedAt))
            .ToListAsync(cancellationToken);

        if (bids == null)
            throw new Exception("Bids not found.");
        
        return bids;
    }
}