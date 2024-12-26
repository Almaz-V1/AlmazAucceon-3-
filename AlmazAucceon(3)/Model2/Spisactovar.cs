using System;
using System.Collections.Generic;

namespace AlmazAucceon_3_.Model2;

public partial class Spisactovar
{
    public int IdSpisacTovars { get; set; }

    public int IdUser { get; set; }

    public int IdProdyct { get; set; }

    public int IdCategoris { get; set; }

    public int Count { get; set; }

    public virtual Item IdProdyctNavigation { get; set; } = null!;

    public virtual User IdUserNavigation { get; set; } = null!;
}
