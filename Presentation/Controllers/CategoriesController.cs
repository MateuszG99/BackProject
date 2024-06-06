using Application.Interfaces;
using Domain.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CategoriesController(IApplicationContext dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(dbContext.Categories.ToList());

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var category = dbContext.Categories.Find(id);
            return category is null ? NotFound() : Ok(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = category.CategoryId }, category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Category input)
        {
            var category = dbContext.Categories.Find(id);

            if (category is null) 
                return NotFound();

            category.CategoryName = input.CategoryName;
            dbContext.SaveChanges();

            return Ok(category);
        }

        [Authorize(Roles = Domain.Constants.Administrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = dbContext.Categories.Find(id);

            if (category is null) 
                return NotFound();

            dbContext.Categories.Remove(category);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
