using Nest;
using System.Linq;
using System.Threading.Tasks;

namespace Todos.Dal
{
    internal class TodosRepository : ITodosRepository
    {
        private readonly ElasticClient elasticClient;

        public TodosRepository(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<SearchTodoResult> Search(int? userId = null, string searchExpression = null)
        {
            var result = await elasticClient
                                .SearchAsync<Entities.TodoItem>(searchDescriptor =>
                                    searchDescriptor
                                    .Query(queryDescriptor => filterForUserId(userId, queryDescriptor) && filterForText(searchExpression, queryDescriptor))
                                    .Size(5));

            return new SearchTodoResult(
                items: result.Hits.Select(i => i.Source.ToDomain(i.Id)).ToList(),
                count: result.Total
                );
        }

        private static QueryContainer filterForText(string searchExpression, QueryContainerDescriptor<Entities.TodoItem> queryDescriptor)
        {
            if (string.IsNullOrEmpty(searchExpression))
                return queryDescriptor.MatchAll();
            else
                return queryDescriptor.Match(match => match.Field(f => f.Title).Query(searchExpression));
        }

        private static QueryContainer filterForUserId(int? userId, QueryContainerDescriptor<Entities.TodoItem> queryDescriptor)
        {
            if (userId == null)
                return queryDescriptor.MatchAll();
            else
                return queryDescriptor.Term(term => term.Field(f => f.UserId).Value(userId.Value));
        }

        public async Task<TodoItem> FindById(string id)
        {
            var result = await elasticClient.GetAsync<Entities.TodoItem>(id);
            if (!result.Found)
                return null;
            else
                return result.Source.ToDomain(result.Id);
        }

        public async Task<TodoItem> Insert(CreateNewTodoRequest value)
        {
            var result = await elasticClient.IndexDocumentAsync(value.ToDal());
            return await FindById(result.Id);
        }

        public async Task<TodoItem> Update(string id, EditTodoRequest value)
        {
            var result = await elasticClient.UpdateAsync<Entities.TodoItem, Entities.TodoItemChanges>(id, a => a.Doc(value.ToDal()).DocAsUpsert(false));
            if (result.Result == Result.Updated)
                return await FindById(result.Id);
            else
                return null;
        }

        public async Task<bool> Delete(string id)
        {
            var result = await elasticClient.DeleteAsync<Entities.TodoItem>(id);
            return result.Result == Result.Deleted;
        }
    }
}
