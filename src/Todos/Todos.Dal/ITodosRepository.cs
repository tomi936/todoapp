using System.Threading.Tasks;

namespace Todos.Dal
{
    public interface ITodosRepository
    {
        Task<SearchTodoResult> Search(int? userId = null, string searchExpression = null);
        Task<TodoItem> FindById(string id);
        Task<TodoItem> Insert(CreateNewTodoRequest value);
        Task<TodoItem> Update(string id, EditTodoRequest value);
        Task<bool> Delete(string id);
    }
}
