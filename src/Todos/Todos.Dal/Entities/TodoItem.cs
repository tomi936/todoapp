namespace Todos.Dal.Entities
{
    /// <summary>
    /// Entity class as stored in Elasticsearch database.
    /// Does NOT contain id; Elasticsearch has a special id property inherently,
    /// see https://github.com/elastic/elasticsearch-net/issues/3350#issuecomment-410618969
    /// </summary>
    internal class TodoItem
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public bool Completed { get; set; }
    }
}
