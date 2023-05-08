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
    public class MenusController : ControllerBase
    {
        private readonly RestaurantDBContext _context;

        public MenusController(RestaurantDBContext context)
        {
            _context = context;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
        {
          if (_context.Menus == null)
          {
              return NotFound();
          }
          List<Menu> menus = new List<Menu>();

            foreach (var menu in _context.Menus)
            {
                if(menu.IsDeleted==false)
                {
                    menus.Add(menu);
                   
                }

            }
            return menus;
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu(int id)
        {
          if (_context.Menus == null)
          {
              return NotFound();
          }
            var menu = await _context.Menus.FindAsync(id);
            
            {
                if (menu == null|| menu.IsDeleted==true)
                {
                    return NotFound();
                }
               
            }

            return menu;
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(int id, Menu menu)
        {
            if (id != menu.MenuId)
            {
                return BadRequest();
            }

            _context.Entry(menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
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

        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
          if (_context.Menus == null)
          {
              return Problem("Entity set 'RestaurantDBContext.Menus'  is null.");
          }
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenu", new { id = menu.MenuId }, menu);
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            if (_context.Menus == null)
            {
                return NotFound();
            }
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null || menu.IsDeleted==true)
            {
                return NotFound();
            }
            List<MenuCategory> menuCategories = await _context.MenuCategories.ToListAsync();
            List<int?> filteredCategories = new List<int?>();
            foreach (var menuCategory in menuCategories)
            {
                if (menuCategory.MenuId == id && menuCategory.IsDeleted == false)
                {
                    filteredCategories.Add(menuCategory.CategoryId);
                    menuCategory.IsDeleted = true;
                    _context.MenuCategories.Update(menuCategory);
                    _context.SaveChanges();
                    //List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();
                    //foreach (var categoryDish in categoryDishes)
                    //{
                    //    if()
                    //}

                }
                //List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();

            }

            List<Category> categories = await _context.Categories.ToListAsync();
            categories.ForEach(async category =>
            {
                if (filteredCategories.Contains(category.CategoryId))
                {
                    category.IsDeleted = true;
                    _context.Categories.Update(category);
                    _context.SaveChanges();
                    List<CategoryDish> categoryDishes = await _context.CategoryDishes.ToListAsync();
                    foreach (var categoryDish in categoryDishes)
                    {
                        if (category.CategoryId == categoryDish.CategoryId)
                        {
                            categoryDish.IsDeleted = true;
                            _context.CategoryDishes.Update(categoryDish);
                            _context.SaveChanges();

                            List<Dish> dishes = await _context.Dishes.ToListAsync();
                            foreach (var dish in dishes)
                            {
                                if (categoryDish.DishId == dish.DishId)
                                {
                                    dish.IsDeleted = true;
                                    _context.Dishes.Update(dish);
                                    _context.SaveChanges();
                                }
                            }
                        }
                    }
                }
            });

            menu.IsDeleted = true;
            _context.Menus.Update(menu);
            _context.SaveChanges();


            //_context.Menus.Remove(menu);
            //await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuExists(int id)
        {
            return (_context.Menus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }
    }
}
