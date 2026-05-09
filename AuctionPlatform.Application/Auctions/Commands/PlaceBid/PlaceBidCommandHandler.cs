using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.PlaceBid;

public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, bool>
{
    private readonly IApplicationDbContext _context;
    
    public PlaceBidCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var auction = await _context.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == request.AuctionId, cancellationToken); 
        
        if (auction == null)
            throw new Exception("Auction not found.");
        
        if (DateTime.UtcNow > auction.EndsAt)
            throw new Exception("This auction is already closed.");

        if (request.Amount <= auction.CurrentPrice)
            throw new Exception("Bid amount must be greater than the current price.");
        
        auction.UpdatePriceAndWinner(request.Amount, request.BidderId);
        
        var bid = new Bid(auction.Id, request.BidderId, request.Amount);
        
        _context.Bids.Add(bid);
        
        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("Someone else placed a bid at the exact same time. Please refresh the page and try again.");
        }
    }
}