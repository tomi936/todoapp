using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Todos.Dal;

namespace Todos.Api.Services
{
    public class UsersDeleteBackgroundService : BackgroundService
    {
        private readonly IDatabase redisDb;
        private readonly ITodosRepository repository;

        public UsersDeleteBackgroundService(ConnectionMultiplexer redisConnection, ITodosRepository repository)
        {
            this.redisDb = redisConnection.GetDatabase();
            this.repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // TODO 5. feladat
        }
    }
}
