using System;
using System.Collections.Generic;

namespace AlmazAucceon_3_.Model2;

public partial class User
{
    public int IdUser { get; set; }

    public string UserName { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Patronymic { get; set; }

    public string Psswords { get; set; } = null!;

    public string Fullname => $"{Lastname} {UserName} {Patronymic}";

    public string Login { get; set; } = null!;

    public int IdRole { get; set; }

    public string Money { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime DataUser { get; set; }

    public string Email { get; set; } = null!;

    public virtual Role IdRoleNavigation { get; set; } = null!;

    public virtual ICollection<Spisactovar> Spisactovars { get; set; } = new List<Spisactovar>();
}
