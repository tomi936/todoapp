using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todos.Dal;

namespace Todos.Api.Controllers
{
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
        public async Task<IReadOnlyList<TodoItem>> Get()
        {
            var cachedResult = await cache.TryGet();
            if (cachedResult != null)
            {
                return cachedResult;
            }
            else
            {
                var result = await repository.ListAll();
                await cache.Set(result);
                return result;
            }
        }

        [HttpGet("{id}")]
        public Task<TodoItem> Get(string id)
            => repository.FindById(id);

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TodoItem>> Post([FromBody] CreateNewTodoRequest value)
        {
            var result = await repository.Insert(value);
            await cache.Invalidate();
            return CreatedAtAction(nameof(Get), result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<TodoItem>> Put(string id, [FromBody] EditTodoRequest value)
        {
            var result = await repository.Update(id, value);
            if (result != null)
            {
                await cache.Invalidate();
                return AcceptedAtAction(nameof(Get), result);
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
                await cache.Invalidate();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}
