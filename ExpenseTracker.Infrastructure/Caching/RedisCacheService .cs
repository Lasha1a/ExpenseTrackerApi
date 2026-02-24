using ExpenseTracker.Application.Interfaces.Caching; //
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Caching.Distributed; // The IDistributedCache interface provides a distributed cache implementation that can be used to store and retrieve data across multiple servers or instances. It abstracts away the underlying caching mechanism, allowing you to switch between different caching providers (like Redis, SQL Server, etc.) without changing your application code. In this case, we are using the Redis implementation of IDistributedCache to interact with a Redis cache server.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json; // The System.Text.Json namespace provides functionality for serializing and deserializing JSON data. In this code, we use the JsonSerializer class from this namespace to convert objects to JSON strings when storing them in the cache and to convert JSON strings back to objects when retrieving them from the cache.
using System.Threading.Tasks;

namespace ExpenseTracker.Infrastructure.Caching;

public class RedisCacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public RedisCacheService(IDistributedCache cache)
    {
        _cache = cache;
    }

    // The SetAsync method takes a key, a value of any type, and an optional TimeSpan for expiration. It serializes the value to a JSON string and stores it in the cache with the specified key and expiration time.
    public async Task<T?> GetAsync<T> (string key)
    {
        var cachedValue = await _cache.GetStringAsync(key);

        if(cachedValue is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(cachedValue);
    }

    //The SetAsync method is responsible for storing a value in the cache with a specified key and expiration time. It first serializes the value to a JSON string using the JsonSerializer.Serialize method. Then, it creates an instance of DistributedCacheEntryOptions to specify the expiration time for the cache entry. Finally, it uses the SetStringAsync method of the IDistributedCache interface to store the serialized value in the cache with the provided key and options.
    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var serializedValue = JsonSerializer.Serialize(value);

        // The DistributedCacheEntryOptions class is used to specify options for cache entries, such as expiration time. In this code, we set the AbsoluteExpirationRelativeToNow property to the provided expiration TimeSpan, which means that the cache entry will expire after the specified duration from the time it was added to the cache.
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await _cache.SetStringAsync(key, serializedValue, options); // The SetStringAsync method of the IDistributedCache interface is used to store a string value in the cache with the specified key and options. In this case, we are storing the serialized JSON string representation of the value in the cache with the provided key and expiration options.
    }

    // The RemoveAsync method is responsible for removing a cache entry with the specified key from the cache. It uses the RemoveAsync method of the IDistributedCache interface to delete the cache entry associated with the provided key.
    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}
