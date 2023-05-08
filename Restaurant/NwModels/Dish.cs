using System;
using System.Collections.Generic;

namespace Restaurant.NwModels
{
    public partial class Dish
    {
        public Dish()
        {
            CategoryDishes = new HashSet<CategoryDish>();
        }

        public int DishId { get; set; }
        public string DishName { get; set; } = null!;
        public string DishDescription { get; set; } = null!;
        public decimal DishPrice { get; set; }
        public string DishImage { get; set; } = null!;
        public string Nature { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<CategoryDish> CategoryDishes { get; set; }
    }
}
