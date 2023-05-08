using System;
using System.Collections.Generic;

namespace Restaurant.NwModels
{
    public partial class CategoryDish
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? DishId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Dish? Dish { get; set; }
    }
}
