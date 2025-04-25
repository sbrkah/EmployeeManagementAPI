using EmployeeManagementAPI.Models.DTO;
using System;
using System.Collections.Generic;

namespace EmployeeManagementAPI.Models;

public partial class StStatus
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public decimal? IsDeleted { get; set; }

    public ResStatusDTO ResConvert()
    {
        return new ResStatusDTO()
        {
            Id = Id,
            Name = Name,
        };
    }
}
