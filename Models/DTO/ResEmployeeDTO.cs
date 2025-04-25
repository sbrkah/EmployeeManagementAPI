namespace EmployeeManagementAPI.Models.DTO
{
    public class ResEmployeeDTO
    {
        public string Id { get; set; } = null!;

        public string? Name { get; set; }

        public string? Class { get; set; }

        public decimal? Age { get; set; }

        public decimal? Salary { get; set; }

        public string? Status { get; set; }
    }
}
