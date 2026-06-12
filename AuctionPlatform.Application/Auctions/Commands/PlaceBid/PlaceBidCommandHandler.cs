using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.PlaceBid;

public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    
    public PlaceBidCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    
    public async Task<bool> Handle(PlaceBidCommand request, CancellationToken cancellationToken)
    {
        var auth0Id = _currentUserService.Auth0Id;
        
        if (string.IsNullOrEmpty(auth0Id))
            throw new UnauthorizedAccessException("You must be logged in to place a bid.");
        
        var auction = await _context.AuctionItems
            .FirstOrDefaultAsync(a => a.Id == request.AuctionId, cancellationToken); 
        
        if (auction == null)
            throw new NotFoundException(nameof(AuctionItem), request.AuctionId);
        
        var bidder = await _context.Users
            .Include(u => u.Transactions)
            .FirstOrDefaultAsync(u => u.Auth0Id == auth0Id, cancellationToken);
        
        if (bidder == null)
            throw new NotFoundException(nameof(User), auth0Id);
            
        if (auction.SellerId == bidder.Id)
            throw new Exception("You cannot bid on your own auction.");
        
        var previousWinnerId = auction.WinnerId;
        var previousPrice = auction.CurrentPrice;
        
        auction.UpdatePriceAndWinner(request.Amount, bidder.Id);

        if (previousWinnerId.HasValue)
        {
            if (previousWinnerId.Value == bidder.Id)
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
        
        var bid = new Bid(request.AuctionId, bidder.Id, request.Amount);
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