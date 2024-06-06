using Application.Interfaces;
using Domain.DataModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ShopsController(IApplicationContext dbContext) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll() => Ok(dbContext.Shops.ToList());

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var shop = dbContext.Shops.Include(o => o.Owner).Include(c => c.Category).FirstOrDefault(dbId => dbId.ShopId == id);
            return shop is null ? NotFound() : Ok(shop);
        }

        [HttpPost]
        public IActionResult Create(Shop shop)
        {
            dbContext.Shops.Add(shop);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = shop.ShopId }, shop);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, Shop input)
        {
            var shop = dbContext.Shops.Find(id);

            if (shop is null)
                return NotFound();

            shop.ShopName = input.ShopName;
            shop.ShopAddress = input.ShopAddress;
            shop.OwnerId = input.OwnerId;
            shop.CategoryId = input.CategoryId;

            dbContext.SaveChanges();

            return Ok(shop);
        }

        [Authorize(Roles = Domain.Constants.Administrator)]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var shop = dbContext.Shops.Find(id);

            if (shop is null)
                return NotFound();

            dbContext.Shops.Remove(shop);
            dbContext.SaveChanges();

            return NoContent();
        }
    }
}
