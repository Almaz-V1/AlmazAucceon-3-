using System;
using System.Collections.Generic;

namespace AlmazAucceon_3_.Model2;

public partial class Item
{
    public int ItemId { get; set; }

    public string ItemName { get; set; } = null!;

    public int CategotiaId { get; set; }

    public int CurrentPrice { get; set; }

    public byte[]? ImageItems { get; set; }

    public sbyte? IsDeleted { get; set; }

    public virtual Category Categotia { get; set; } = null!;

    public virtual ICollection<Spisactovar> Spisactovars { get; set; } = new List<Spisactovar>();
}
