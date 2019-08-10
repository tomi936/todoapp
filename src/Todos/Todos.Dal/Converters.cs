namespace Todos.Dal
{
    internal static class Converters
    {
        public static TodoItem ToDomain(this Entities.TodoItem value, string id)
            => new TodoItem(id, value.UserId, value.Title, value.Completed);

        public static Entities.TodoItem ToDal(this CreateNewTodoRequest value)
            => new Entities.TodoItem()
            {
                UserId = value.UserId,
                Title = value.Title,
                Completed = value.Completed
            };

        public static Entities.TodoItemChanges ToDal(this EditTodoRequest value)
            => new Entities.TodoItemChanges()
            {
                Completed = value.Completed
            };
    }
}
