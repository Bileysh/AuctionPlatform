namespace AuctionPlatform.Application.Common.Interfaces;

public interface IOwnedResourceRequest
{
    Guid ResourceId { get; }
    ResourceType Type { get; }
}