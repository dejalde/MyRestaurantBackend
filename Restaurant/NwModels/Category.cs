using System;
using System.Collections.Generic;

namespace Restaurant.NwModels
{
    public partial class Category
    {
        public Category()
        {
            CategoryDishes = new HashSet<CategoryDish>();
            MenuCategories = new HashSet<MenuCategory>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string CategoryDescription { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<CategoryDish> CategoryDishes { get; set; }
        public virtual ICollection<MenuCategory> MenuCategories { get; set; }
    }
}
