using System;
using System.Collections.Generic;

namespace Restaurant.NwModels
{
    public partial class MenuCategory
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public int? CategoryId { get; set; }
        public bool IsDeleted { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Menu? Menu { get; set; }
    }
}
