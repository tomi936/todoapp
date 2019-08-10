using System.Collections.Generic;

namespace Todos
{
    public class SearchTodoResult
    {
        public SearchTodoResult(IReadOnlyList<TodoItem> items, long count)
        {
            Items = items;
            Count = count;
        }

        public IReadOnlyList<TodoItem> Items { get; }
        public long Count { get; }
    }
}
