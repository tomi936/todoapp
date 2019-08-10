using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Todos.Dal;

namespace Todos.Api.Controllers
{
    /// <summary>
    /// Handles all requests sent to /api/todos
    /// </summary>
    [Route("api/todos")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodosRepository repository;
        private Cache.TodosCache cache;

        public TodosController(ITodosRepository repository, Cache.TodosCache cache)
        {
            this.repository = repository;
            this.cache = cache;
        }

        [HttpGet]
        public async Task<SearchTodoResult> Search([FromQuery] int? userId = null, [FromQuery] string searchExpression = null)
        {
            // TODO 4. feladat
            return new SearchTodoResult(
                new[] { new TodoItem("a", 1, "Test item", false) },
                5);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItem>> Get(string id)
        {
            // TODO 4. feladat
            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TodoItem>> Post([FromBody] CreateNewTodoRequest value)
        {
            var result = await repository.Insert(value);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItem>> Put(string id, [FromBody] EditTodoRequest value)
        {
            var result = await repository.Update(id, value);
            if (result != null)
            {
                await cache.Invalidate(id);
                return AcceptedAtAction(nameof(Get), new { id }, result);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(string id)
        {
            var deleted = await repository.Delete(id);
            if (deleted)
            {
                await cache.Invalidate(id);
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
