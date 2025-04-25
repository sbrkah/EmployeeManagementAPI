namespace EmployeeManagementAPI.Models.DTO
{
    public class ReqStatusDTO
    {
        public string? Name { get; set; }

        public decimal? IsDeleted { get; set; } = 0;

        public StStatus MainConvert()
        {
            return new StStatus()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                IsDeleted = IsDeleted
            };
        }
    }
}
