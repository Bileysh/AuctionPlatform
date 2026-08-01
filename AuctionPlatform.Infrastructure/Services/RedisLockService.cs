using AuctionPlatform.Application.Common.Interfaces;
using StackExchange.Redis;

namespace AuctionPlatform.Infrastructure.Services;

public class RedisLockService: IDistributedLockService
{
    private readonly IDatabase _redisDb;

    public RedisLockService(IConnectionMultiplexer redis)
    {
        _redisDb = redis.GetDatabase();
    }

    public async Task<bool> AcquireLockAsync(string resource, string token, TimeSpan expiration)
    {
        return await _redisDb.LockTakeAsync(resource, token, expiration);
    }

    public async Task ReleaseLockAsync(string resource, string token)
    {
        await _redisDb.LockReleaseAsync(resource, token);
    }
}