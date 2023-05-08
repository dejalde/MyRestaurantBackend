using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant.NwModels;

namespace Restaurant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishesController : ControllerBase
    {
        private readonly RestaurantDBContext _context;

        public DishesController(RestaurantDBContext context)
        {
            _context = context;
        }

        // GET: api/Dishes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
        {
          if (_context.Dishes == null)
          {
              return NotFound();
          }
            //return await _context.Dishes.ToListAsync();
            List<Dish> dishes = new List<Dish>();

            foreach (var dish in _context.Dishes)
            {
                if (dish.IsDeleted == false)
                {
                    dishes.Add(dish);

                }

            }
            return dishes;
        }

        // GET: api/Dishes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dish>> GetDish(int id)
        {
          if (_context.Dishes == null)
          {
              return NotFound();
          }
            var dish = await _context.Dishes.FindAsync(id);

            if (dish == null||dish.IsDeleted==true)
            {
                return NotFound();
            }

            return dish;
        }

        //To get dishes under specific category
        [HttpGet("categoryId = {categoryId}")]
        public async Task<ActionResult<IEnumerable<Dish>>> GetDishes(int categoryId)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }



            List<CategoryDish>categoryDish = await _context.CategoryDishes.ToListAsync();
            List<int?> filteredList = new List<int?>();



            categoryDish.FindAll(categoryDish => categoryDish.IsDeleted == false && categoryDish.CategoryId == categoryId).
            ForEach(catDish => filteredList.Add(catDish.DishId));



            List<Dish> dishes = new List<Dish>();
            await _context.Dishes.ForEachAsync(dish =>
            {
                if (dish.IsDeleted == false && filteredList.Contains(dish.DishId))
                {
                    dishes.Add(dish);
                }
            });



            return Ok(dishes);
        }

        // PUT: api/Dishes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDish(int id, Dish dish)
        {
            if (id != dish.DishId)
            {
                return BadRequest();
            }

            _context.Entry(dish).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DishExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Dishes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{categoryId}")]
        public async Task<ActionResult<Dish>> PostDish(int categoryId, Dish dish)
        {
          if (_context.Dishes == null)
          {
              return Problem("Entity set 'RestaurantDBContext.Dishes'  is null.");
          }
            _context.Dishes.Add(dish);
            await _context.SaveChangesAsync();


            //updating CategoryDish table
            CategoryDish categoryDish = new CategoryDish();
            categoryDish.CategoryId = categoryId;
            categoryDish.DishId = dish.DishId;
            _context.CategoryDishes.Add(categoryDish);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDish", new { id = dish.DishId }, dish);
        }


        // DELETE: api/Dishes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDish(int id)
        {
            if (_context.Dishes == null)
            {
                return NotFound();
            }
            var dish = await _context.Dishes.FindAsync(id);
            if (dish == null && dish.IsDeleted==true)
            {
                return NotFound();
            }

            CategoryDish? cd = _context.CategoryDishes.ToList().Find(mc => mc.DishId == id);
            if (cd != null)
            {
                cd.IsDeleted = true;
                _context.CategoryDishes.Update(cd);
                _context.SaveChanges();
            }



            dish.IsDeleted = true;



            _context.Dishes.Update(dish);
            _context.SaveChanges();

            return NoContent();
        }

        private bool DishExists(int id)
        {
            return (_context.Dishes?.Any(e => e.DishId == id)).GetValueOrDefault();
        }
    }
}
