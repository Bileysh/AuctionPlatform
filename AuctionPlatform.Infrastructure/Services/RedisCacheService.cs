using System.Text.Json;
using AuctionPlatform.Application.Common.Interfaces;
using StackExchange.Redis;

namespace AuctionPlatform.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _redisDb;
    private readonly IConnectionMultiplexer _redis;

    public RedisCacheService(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _redisDb = redis.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
    {
        var value = await _redisDb.StringGetAsync(key);
        if (value.IsNull) return default;
        return JsonSerializer.Deserialize<T>(value!);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expirationTime = null, CancellationToken cancellationToken = default)
    {
        var serializedValue = JsonSerializer.Serialize(value);
        await _redisDb.StringSetAsync(key, serializedValue, (Expiration)expirationTime!);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
    {
        await _redisDb.KeyDeleteAsync(key);
    }

    public async Task RemoveByPrefixAsync(string prefixKey, CancellationToken cancellationToken = default)
    {
        var endpoints = _redis.GetEndPoints();
        var server = _redis.GetServer(endpoints.First());
        
        var keys = server.Keys(pattern: $"{prefixKey}*").ToArray();
        
        if (keys.Any())
        {
            await _redisDb.KeyDeleteAsync(keys);
        }
    }
}