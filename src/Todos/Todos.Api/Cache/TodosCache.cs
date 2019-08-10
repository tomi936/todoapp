using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace Todos.Api.Cache
{
    public class TodosCache
    {
        private readonly IDistributedCache cache;
        private readonly DistributedCacheEntryOptions cacheEntryOptions;

        public TodosCache(IDistributedCache cache)
        {
            this.cache = cache;
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
            var valueAsString = Newtonsoft.Json.JsonConvert.SerializeObject(value);
            await cache.SetStringAsync(key: getCacheKey(value.Id), value: valueAsString, options: cacheEntryOptions);
        }

        public Task Invalidate(string todoItemId) => cache.RemoveAsync(getCacheKey(todoItemId));

        private string getCacheKey(string todoItemId) => $"todos-{todoItemId}";
    }
}
