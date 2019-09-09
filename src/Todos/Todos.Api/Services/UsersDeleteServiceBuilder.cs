using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Todos.Api.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class UsersDeleteServiceBuilder
    {
        public static void AddUsersDeleteService(this IServiceCollection services, IConfiguration config)
        {
            var redisUrl = config.GetValue<string>("RedisUrl") ?? "redis:6379";

            services.AddSingleton(_ => ConnectionMultiplexer.Connect(redisUrl));
            services.AddHostedService<UsersDeleteBackgroundService>();
        }
    }
}
