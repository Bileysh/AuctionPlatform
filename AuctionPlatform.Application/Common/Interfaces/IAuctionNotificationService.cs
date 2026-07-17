namespace AuctionPlatform.Application.Common.Interfaces;

public interface IAuctionNotificationService
{
    Task SendNewBidAsync(Guid auctionId, decimal newAmount, CancellationToken cancellationToken = default);
    Task SendAuctionCreatedAsync(Guid auctionId, CancellationToken cancellationToken = default);
    Task SendAuctionPriceUpdatedAsync(Guid auctionId, decimal newPrice, CancellationToken cancellationToken = default); 
    Task SendAuctionClosedAsync(Guid auctionId, CancellationToken cancellationToken = default);

}