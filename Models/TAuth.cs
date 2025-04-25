using System;
using System.Collections.Generic;

namespace EmployeeManagementAPI.Models;

public partial class TAuth
{
    public string Id { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? EmployeeId { get; set; }
}
