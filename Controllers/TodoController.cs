using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using webapi.Models;

namespace webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
            if (_context.Todoitems.Count() == 0)
            {
                _context.Todoitems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public List<TodoItem> GetAll()
        {
            return _context.Todoitems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var item = _context.Todoitems.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        [HttpPost]
        public IActionResult Create(TodoItem item)
        {
            _context.Todoitems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTodo", new TodoItem { Id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, TodoItem item)
        {
            var todo = _context.Todoitems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.Todoitems.Update(todo);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.Todoitems.Find(id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.Todoitems.Remove(todo);
            _context.SaveChanges();
            return NoContent();
        }
    }
}