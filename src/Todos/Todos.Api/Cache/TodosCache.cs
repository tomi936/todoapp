using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todos.Api.Cache
{
    public class TodosCache
    {
        private readonly IDistributedCache cache;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;
        private const string cacheKey = "todos-search-result";

        public TodosCache(IDistributedCache cache)
        {
            this.cache = cache;
            this.cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1));
        }

        public Task Invalidate()
            => cache.RemoveAsync(cacheKey);

        public async Task<IReadOnlyList<TodoItem>> TryGet()
        {
            var cachedValue = await cache.GetStringAsync(cacheKey);
            if (cachedValue == null)
                return null;
            else
                return Newtonsoft.Json.JsonConvert.DeserializeObject<IReadOnlyList<TodoItem>>(cachedValue);
        }

        public async Task Set(IReadOnlyList<TodoItem> value)
        {
            var valueAsString = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(cacheKey, valueAsString, cacheEntryOptions);
        }
    }
}
