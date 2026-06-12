namespace AuctionPlatform.Application.Common.Interfaces;

public interface ICurrentUserService
{
    string? Auth0Id { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}