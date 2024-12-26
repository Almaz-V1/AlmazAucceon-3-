using System;
using System.Collections.Generic;

namespace AlmazAucceon_3_.Model2;

public partial class Role
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Staffe> Staffes { get; set; } = new List<Staffe>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
