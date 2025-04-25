using EmployeeManagementAPI.Models.DTO;
using System;
using System.Collections.Generic;

namespace EmployeeManagementAPI.Models;

public partial class TEmployee
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Class { get; set; }

    public decimal? Age { get; set; }

    public decimal? Salary { get; set; }

    public decimal? IsDeleted { get; set; }

    public string? Status { get; set; }

    public ResEmployeeDTO ResConvert()
    {
        return new ResEmployeeDTO()
        {
            Id = Id,
            Name = Name,
            Class = Class,
            Age = Age,
            Salary = Salary,
            Status = Status,
        };
    }
}
