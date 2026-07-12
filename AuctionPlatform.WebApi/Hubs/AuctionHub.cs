using Microsoft.AspNetCore.SignalR;

namespace AuctionPlatform.WebApi.Hubs;

public class AuctionHub: Hub
{
    public async Task JoinAuctionGroup(string auctionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"Auction-{auctionId}");
    }
    
    public async Task LeaveAuctionGroup(string auctionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Auction-{auctionId}");
    }
    
    public async Task JoinActiveAuctionsGroup()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "ActiveAuctions");
    }

    public async Task LeaveActiveAuctionsGroup()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ActiveAuctions");
    }
}