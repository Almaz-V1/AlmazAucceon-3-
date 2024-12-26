using System;
using System.Collections.Generic;

namespace AlmazAucceon_3_.Model2;

public partial class Staffe
{
    public int IdStaffes { get; set; }

    public string StaffName { get; set; } = null!;

    public string StaffLastname { get; set; } = null!;

    public string? StaffPatronymic { get; set; }

    public string StaffPsswords { get; set; } = null!;

    public string StaffLogin { get; set; } = null!;

    public int IdStaffRole { get; set; }

    public string PhoneNumber { get; set; } = null!;

    public DateTime Data { get; set; }

    public string Email { get; set; } = null!;

    public virtual Role IdStaffRoleNavigation { get; set; } = null!;
}
