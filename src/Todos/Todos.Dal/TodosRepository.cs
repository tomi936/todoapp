using Nest;
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
            // TODO 4. feladat
            return new SearchTodoResult(
                new[] { new TodoItem("a", userId ?? 1, searchExpression ?? "test", false) }, 1);
        }

        private static QueryContainer filterForText(string searchExpression, QueryContainerDescriptor<Entities.TodoItem> queryDescriptor)
            => string.IsNullOrEmpty(searchExpression)
                ? queryDescriptor.MatchAll()
                : queryDescriptor.Match(match => match.Field(f => f.Title).Query(searchExpression)); //  Match query will look for similar texts (full text search)

        private static QueryContainer filterForUserId(int? userId, QueryContainerDescriptor<Entities.TodoItem> queryDescriptor)
            => userId == null
                ? queryDescriptor.MatchAll()
                : queryDescriptor.Term(term => term.Field(f => f.UserId).Value(userId.Value)); // a Term query will look for eact match

        public async Task<TodoItem> FindById(string id)
        {
            // TODO 4. feladat
            return null;
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
