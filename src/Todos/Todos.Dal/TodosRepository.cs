using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;

namespace Todos.Dal
{
    internal class TodosRepository : ITodosRepository
    {
        private readonly ElasticClient elasticClient;

        public TodosRepository(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public async Task<IReadOnlyList<TodoItem>> ListAll()
        {
            var result = await elasticClient.SearchAsync<Entities.TodoItem>();
            return result.Hits.Select(i => i.Source.ToDomain(i.Id)).ToList();
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
