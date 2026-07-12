using AuctionPlatform.Application.Common.Interfaces;
using AuctionPlatform.WebApi.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace AuctionPlatform.WebApi.Infrastructure;

public class AuctionNotificationService: IAuctionNotificationService
{
    private readonly IHubContext<AuctionHub> _hubContext;

    public AuctionNotificationService(IHubContext<AuctionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendNewBidAsync(Guid auctionId, decimal newAmount, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group($"Auction-{auctionId}")
            .SendAsync("ReceiveNewBid", newAmount, cancellationToken);
    }

    public async Task SendAuctionCreatedAsync(Guid auctionId, CancellationToken cancellationToken = default)
    {
        await  _hubContext.Clients
            .Group("ActiveAuctions")
            .SendAsync("AuctionCreated", auctionId, cancellationToken);
    }
    
    public async Task SendAuctionPriceUpdatedAsync(Guid auctionId, decimal newPrice, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group("ActiveAuctions") 
            .SendAsync("AuctionPriceUpdated", auctionId, newPrice, cancellationToken);
    }

}