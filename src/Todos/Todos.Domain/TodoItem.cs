namespace Todos
{
    public class TodoItem
    {
        public TodoItem(string id, int userId, string title, bool completed)
        {
            UserId = userId;
            Id = id;
            Title = title;
            Completed = completed;
        }

        public string Id { get; }
        public int UserId { get; }
        public string Title { get; }
        public bool Completed { get; }
    }
}
