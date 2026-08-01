namespace AuctionPlatform.Application.Common.Interfaces;

public interface IDistributedLockService
{
    Task<bool> AcquireLockAsync(string resource, string token, TimeSpan expiration);
    Task ReleaseLockAsync(string resource, string token); 
}