namespace AuctionPlatform.Application.Common.Interfaces;

public interface IOwnershipChecker
{
    Task<bool> IsOwnerAsync(Guid resourceId, ResourceType type, string auth0Id, CancellationToken cancellationToken);
}