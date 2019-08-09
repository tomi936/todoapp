using System;
using Microsoft.Extensions.Configuration;
using Nest;
using Todos.Dal;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TodosDalBuilder
    {
        public static void AddTodosDal(this IServiceCollection services, IConfiguration config)
        {
            var esConfig = getElasticConnectionConfig(config);
            services.AddSingleton(new ElasticClient(esConfig));

            services.AddTransient<ITodosRepository, TodosRepository>();
        }

        private static ConnectionSettings getElasticConnectionConfig(IConfiguration config)
        {
            var esUrl = config.GetValue<string>("ElasticsearchUrl") ?? "http://elasticsearch:9200";
            return new ConnectionSettings(new Uri(esUrl))
                .ThrowExceptions()
                .DefaultMappingFor<Todos.Dal.Entities.TodoItem>(
                    m => m.IndexName("todos"));
        }
    }
}
