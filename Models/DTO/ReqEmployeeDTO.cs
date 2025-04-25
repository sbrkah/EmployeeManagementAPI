namespace EmployeeManagementAPI.Models.DTO
{
    public class ReqEmployeeDTO
    {
        public string? Name { get; set; }

        public string? Class { get; set; }

        public decimal? Age { get; set; }

        public decimal? Salary { get; set; }

        public decimal? IsDeleted { get; set; }

        public string? Status { get; set; }

        public TEmployee MainConvert()
        {
            return new TEmployee()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Class = Class,
                Age = Age,
                Salary = Salary,
                IsDeleted = IsDeleted,
                Status = Status
            };
        }
    }
}
