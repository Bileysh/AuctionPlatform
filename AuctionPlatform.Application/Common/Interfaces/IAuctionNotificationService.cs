namespace AuctionPlatform.Application.Common.Interfaces;

public interface IAuctionNotificationService
{
    Task SendNewBidAsync(Guid auctionId, decimal newAmount, CancellationToken cancellationToken);
    Task SendAuctionCreatedAsync(Guid auctionId, CancellationToken cancellationToken);
    Task SendAuctionPriceUpdatedAsync(Guid auctionId, decimal newPrice, CancellationToken cancellationToken = default); // ← нове

}