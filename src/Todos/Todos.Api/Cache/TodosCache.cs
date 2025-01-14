﻿using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Todos.Api.Cache
{
    /// <summary>
    /// Caches individual todo items by their id.
    /// </summary>
    public class TodosCache
    {
        private readonly IDistributedCache cache;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;

        public TodosCache(IDistributedCache cache)
        {
            this.cache = cache;

            // Every cache entry exires in 1 minute
            // - Keeps the memory pressure of Redis low
            // - A sort of "eventually consistency": if the cache is out of date, it will expire soon.
            this.cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
        }

        public async Task<TodoItem> TryGet(string todoItemId)
        {
            var cacheItem = await cache.GetStringAsync(getCacheKey(todoItemId));
            if (cacheItem == null)
                return null;
            else
                return Newtonsoft.Json.JsonConvert.DeserializeObject<TodoItem>(cacheItem);
        }

        public async Task Set(TodoItem value)
        {
            var valueAsString = Newtonsoft.Json.JsonConvert.SerializeObject(value); // the cache can only store byte[] or string, hence the manual serialization
            await cache.SetStringAsync(key: getCacheKey(value.Id), value: valueAsString, options: cacheEntryOptions);
        }

        public Task Invalidate(string todoItemId) => cache.RemoveAsync(getCacheKey(todoItemId));

        private string getCacheKey(string todoItemId) => $"todos-{todoItemId}";
    }
}
