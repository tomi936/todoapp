using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todos.Dal
{
    public interface ITodosRepository
    {
        Task<IReadOnlyList<TodoItem>> ListAll();
        Task<TodoItem> FindById(string id);
        Task<TodoItem> Insert(CreateNewTodoRequest value);
        Task<TodoItem> Update(string id, EditTodoRequest value);
        Task<bool> Delete(string id);
    }
}
