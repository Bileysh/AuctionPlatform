using AuctionPlatform.Application.Common.Exceptions;
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
            throw new NotFoundException(nameof(AuctionItem), request.AuctionId);
        
        var bidder = await _context.Users
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.Id == request.BidderId, cancellationToken);
        
        if (bidder == null)
            throw new NotFoundException(nameof(Bid), request.BidderId);
        
        var previousWinnerId = auction.WinnerId;
        var previousPrice = auction.CurrentPrice;
        
        auction.UpdatePriceAndWinner(request.Amount, request.BidderId);

        if (previousWinnerId.HasValue)
        {
            if (previousWinnerId.Value == request.BidderId)
            {
                bidder.ReleaseFunds(previousPrice, auction.Id);
            }
            else
            {
                var previousWinner = await _context.Users
                    .Include(u => u.Transactions)
                    .FirstOrDefaultAsync(u => u.Id == previousWinnerId.Value, cancellationToken);
                
                previousWinner?.ReleaseFunds(previousPrice, auction.Id);
            }
        }
        
        bidder.HoldFunds(request.Amount, auction.Id);
        
        var bid = new Bid(request.AuctionId, request.BidderId, request.Amount);
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