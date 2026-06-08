using AuctionPlatform.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Queries.GetAuctionById;

public class GetAuctionByIdQueryHandler : IRequestHandler<GetAuctionByIdQuery, AuctionDetailsDto>
{
    private readonly IApplicationDbContext _context;
    
    public GetAuctionByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuctionDetailsDto> Handle(GetAuctionByIdQuery request, CancellationToken cancellationToken)
    {
        var auction = await _context.AuctionItems
            .Include(u => u.Bids).ThenInclude(b => b.Bidder)
            .Include(u => u.Category)
            .Include(u => u.Comments).ThenInclude(u => u.Author)
            .Include(u => u.Seller)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == request.Id, cancellationToken);
        
        if (auction == null)          
            throw new Exception("Auction not found.");
        
        return new AuctionDetailsDto(
            auction.Id,
            auction.Title,
            auction.Description,
            auction.CurrentPrice,
            auction.EndsAt,
            auction.Category.Name,
            auction.Seller.UserName,
            auction.Status,
            auction.Bids
                .OrderByDescending(b => b.Amount)
                .Select(b => new BidDto(b.Id, b.Bidder.UserName, b.Amount, b.CreatedAt))
                .ToList(),
            auction.Comments
                .OrderBy(c => c.CreatedAt)
                .Select(c => new CommentDto(c.Id, c.Author.UserName, c.Text, c.CreatedAt))
                .ToList()
            );
        
    }
}