using EmployeeManagementAPI.Models.DTO;
using System;
using System.Collections.Generic;

namespace EmployeeManagementAPI.Models;

public partial class StClass
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public decimal? IsDeleted { get; set; }

    public ResClassDTO ResConvert()
    {
        return new ResClassDTO()
        {
            Id = Id,
            Name = Name
        };
    }
}
