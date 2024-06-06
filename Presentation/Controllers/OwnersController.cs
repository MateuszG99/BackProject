using Application.Interfaces;
using Domain.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class OwnersController(IApplicationContext dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(dbContext.Owners.ToList());

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var owner = dbContext.Owners.Find(id);
            return owner is null ? NotFound() : Ok(owner);
        }

        [HttpPost]
        public IActionResult Create(Owner input)
        {
            dbContext.Owners.Add(input);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = input.OwnerId }, input);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Owner input)
        {
            var owner = dbContext.Owners.Find(id);

            if (owner is null)
                return NotFound();

            owner.Firstname = input.Firstname;
            owner.Surname = input.Surname;
            dbContext.SaveChanges();

            return Ok(owner);
        }

        [Authorize(Roles = Domain.Constants.Administrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var category = dbContext.Owners.Find(id);

            if (category is null)
                return NotFound();

            dbContext.Owners.Remove(category);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
