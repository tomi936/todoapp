using System.ComponentModel.DataAnnotations;

namespace Todos
{
    public class EditTodoRequest
    {
        public EditTodoRequest(string title, bool completed)
        {
            Title = title;
            Completed = completed;
        }

        [Required]
        [MinLength(1)]
        public string Title { get; }

        public bool Completed { get; }
    }
}
