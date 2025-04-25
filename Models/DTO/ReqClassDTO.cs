namespace EmployeeManagementAPI.Models.DTO
{
    public class ReqClassDTO
    {
        public string? Name { get; set; }

        public decimal? IsDeleted { get; set; } = 0;

        public StClass MainConvert()
        {
            return new StClass()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                IsDeleted = IsDeleted
            };
        }
    }
}
