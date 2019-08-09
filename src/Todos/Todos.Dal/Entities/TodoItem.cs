namespace Todos.Dal.Entities
{
    internal class TodoItem
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}
