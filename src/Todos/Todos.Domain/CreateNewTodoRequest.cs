using System.ComponentModel.DataAnnotations;

namespace Todos
{
    public class CreateNewTodoRequest
    {
        public CreateNewTodoRequest(int userId, string title, bool completed)
        {
            UserId = userId;
            Title = title;
            Completed = completed;
        }

        public int UserId { get; }

        [Required]
        [MinLength(1)]
        public string Title { get; }

        public bool Completed { get; }
    }
}
