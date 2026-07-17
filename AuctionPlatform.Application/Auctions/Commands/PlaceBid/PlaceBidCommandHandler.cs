using AuctionPlatform.Application.Common.Exceptions;
using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities;
using AuctionPlatform.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Application.Auctions.Commands.PlaceBid;

public class PlaceBidCommandHandler : IRequestHandler<PlaceBidCommand, bool>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuctionNotificationService _notificationService;
    
    public PlaceBidCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IAuctionNotificationService notificationService)
    {
        _context = context;
        _currentUserService = currentUserService;
        _notificationService = notificationService;
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
            throw new BusinessRuleException("You cannot bid on your own auction.");
        
        var availableBalance = bidder.GetAvailableBalance();
        
        decimal alreadyHeldForThisAuction = 0;
        if (auction.WinnerId == bidder.Id)
        {
            alreadyHeldForThisAuction = auction.CurrentPrice;
        }
        
        var effectiveAvailableBalance = availableBalance + alreadyHeldForThisAuction;
        
        if (request.Amount > effectiveAvailableBalance)
        {
            throw new BusinessRuleException($"Недостатньо коштів. Доступно для цієї ставки: {effectiveAvailableBalance} ₴, сума ставки: {request.Amount} ₴");
        }
        
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
        
        bidder.HoldFunds(request.Amount, auction.Id, effectiveAvailableBalance);
        
        var bid = new Bid(request.AuctionId, bidder.Id, request.Amount);
        _context.Bids.Add(bid);

        await _context.SaveChangesAsync(cancellationToken);
            
        await _notificationService.SendNewBidAsync(request.AuctionId, request.Amount, cancellationToken);
        await _notificationService.SendAuctionPriceUpdatedAsync(request.AuctionId, request.Amount, cancellationToken);

        return true;
    
       
    }
}