using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.Domain.Entities.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuctionPlatform.Application.Auctions.Commands.CloseExpiredAuctions;

public class CloseExpiredAuctionsCommandHandler : IRequestHandler<CloseExpiredAuctionsCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CloseExpiredAuctionsCommandHandler> _logger;
    private readonly IAuctionNotificationService  _notificationService;
    
    public CloseExpiredAuctionsCommandHandler(IApplicationDbContext context, ILogger<CloseExpiredAuctionsCommandHandler> logger, IAuctionNotificationService notificationService)
    {
        _context = context;
        _logger = logger;
        _notificationService = notificationService;
    }
    
    public async Task<int> Handle(CloseExpiredAuctionsCommand request, CancellationToken cancellationToken)
    {
        var expiredAuctions = await _context.AuctionItems
            .Include(a => a.Seller)
            .Include(a => a.Winner)
            .Where(a => a.Status == AuctionStatus.Active && a.EndsAt <= DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        if (!expiredAuctions.Any())
        {
            _logger.LogInformation("No expired auctions found to close.");
            return 0;
        }

        int successfullyClosed = 0;

        foreach (var auction in expiredAuctions)
        {
            try
            {
                auction.Close();
                
                if (auction.WinnerId.HasValue && auction.Winner != null)
                {
                    auction.Winner.PayForWonAuction(auction.CurrentPrice, auction.Id);
                    auction.Seller.ReceiveAuctionIncome(auction.CurrentPrice, auction.Id);
                    
                    _logger.LogInformation(
                        "Auction {AuctionId} closed. Winner: {WinnerName}. Price: {CurrentPrice}", 
                        auction.Id, 
                        auction.Winner.UserName, 
                        auction.CurrentPrice);
                }
                else
                {
                    _logger.LogInformation("Auction {AuctionId} closed with no bids.", auction.Id);
                }
                
                await _notificationService.SendAuctionClosedAsync(auction.Id, cancellationToken);
                
                await _context.SaveChangesAsync(cancellationToken);
                successfullyClosed++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close auction {AuctionId}. Moving to next.", auction.Id);
                
                if (_context is DbContext dbContext)
                {
                    dbContext.Entry(auction).State = EntityState.Unchanged;
                }
                
            }
        }
        
        return successfullyClosed;
    }
}