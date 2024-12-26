using System;
using System.Collections.Generic;

namespace AlmazAucceon_3_.Model2;

public partial class Category
{
    public int IdCategories { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
