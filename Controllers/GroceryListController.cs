using AspnetCore.Jwt.Authentication.DbContexts;
using AspnetCore.Jwt.Authentication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AspnetCore.Jwt.Authentication.Controllers
{
    [Route("api/[controller]")]
    public class GroceryListController: Controller
    {
        private readonly GroceryListContext _context;

        public GroceryListController(GroceryListContext context)
        {
            this._context = context;

            if (this._context.GroceryList.Count() == 0)
            {
                _context.GroceryList.Add(new Models.GroceryItem { Description = "Item1" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<GroceryItem> GetAll()
        {
            return _context.GroceryList.AsNoTracking().ToList();
        }

        [HttpGet("{id}", Name = "GetGroceryItem")]
        public IActionResult GetById(long id)
        {
            var item = _context.GroceryList.FirstOrDefault(g => g.Id == id);

            if (item != null)
            {
                return new ObjectResult(item);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Create([FromBody] GroceryItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.GroceryList.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetGroceryItem", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var item = _context.GroceryList.FirstOrDefault(g => g.Id == id);

            if (item == null)
                return BadRequest();

            _context.GroceryList.Remove(item);
            _context.SaveChanges();

            return new NoContentResult();
        }
    }
}
