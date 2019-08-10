using System.ComponentModel.DataAnnotations;

namespace Todos
{
    public class EditTodoRequest
    {
        public EditTodoRequest(bool completed)
        {
            Completed = completed;
        }

        [Required]
        public bool Completed { get; }
    }
}
