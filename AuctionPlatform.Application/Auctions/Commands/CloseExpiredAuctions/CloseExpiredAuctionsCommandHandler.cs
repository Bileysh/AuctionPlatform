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
    
    public CloseExpiredAuctionsCommandHandler(IApplicationDbContext context, ILogger<CloseExpiredAuctionsCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
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

        foreach (var auction in expiredAuctions)
        {
            auction.Close();
            
            if(auction.WinnerId.HasValue && auction.Winner != null)
            {
                auction.Winner.PayForWonAuction(auction.CurrentPrice, auction.Id);
                auction.Seller.ReceiveAuctionIncome(auction.CurrentPrice, auction.Id);
                
                _logger.LogInformation(
                    "Auction {AuctionId} closed. Winner: {WinnerName}. Price: {CurrentPrice}", 
                    auction.Id, 
                    auction.Winner.UserName, 
                    auction.CurrentPrice);            }
            else
            {
                _logger.LogInformation("Auction {AuctionId} closed with no bids.", auction.Id);            }
        }
        
        await _context.SaveChangesAsync(cancellationToken);
        return expiredAuctions.Count;
    }
}