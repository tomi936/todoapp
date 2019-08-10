namespace Todos.Dal.Entities
{
    /// <summary>
    /// Partial document update. Contains only properties that must be changed.
    /// </summary>
    internal class TodoItemChanges
    {
        public bool Completed { get; set; }
    }
}