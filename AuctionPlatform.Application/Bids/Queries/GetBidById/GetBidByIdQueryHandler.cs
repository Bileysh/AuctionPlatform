using AuctionPlatform.Application.Bids.Queries.GetAllBids;
using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Bids.Queries.GetBidById;

public class GetBidByIdQueryHandler : IRequestHandler<GetBidByIdQuery, BidDto>
{
    private readonly IApplicationDbContext _context;
    
    public GetBidByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<BidDto> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
    {
        var bid = await _context.Bids
            .AsNoTracking()
            .Select(b => new BidDto(b.Id, b.AuctionItemId, b.Bidder.UserName, b.Amount, b.CreatedAt))
            .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

        if (bid == null)
            throw new NotFoundException(nameof(Bid), request.Id);
        
        return bid;
    }
}