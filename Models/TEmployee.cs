using System;
using System.Collections.Generic;

namespace EmployeeManagementAPI.Models;

public partial class TEmployee
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public decimal? Class { get; set; }

    public decimal? Age { get; set; }

    public decimal? Salary { get; set; }

    public decimal? IsDeleted { get; set; }

    public decimal? Status { get; set; }
}
