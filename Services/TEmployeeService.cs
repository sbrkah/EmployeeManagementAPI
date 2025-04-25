using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services
{
    public interface ITEmployeeService
    {
        Task<IEnumerable<ResEmployeeDTO>> GetAllEmployeesAsync();
        Task<ResEmployeeDTO> GetEmployeeByIdAsync(string id);
        Task<ResEmployeeDTO> CreateEmployeeAsync(ReqEmployeeDTO employee);
        Task<ResEmployeeDTO> UpdateEmployeeAsync(string id, ReqEmployeeDTO employee);
        Task DeleteEmployeeAsync(string id);
        Task<bool> EmployeeExistsAsync(string id);
    }

    public class TEmployeeService : ITEmployeeService
    {
        private readonly EmanagerContext _context;

        public TEmployeeService(EmanagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ResEmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _context.TEmployees
                .Where(x => x.IsDeleted == 0)
                .ToListAsync();

            return employees.Select(x => x.ResConvert());
        }

        public async Task<ResEmployeeDTO> GetEmployeeByIdAsync(string id)
        {
            var employee = await _context.TEmployees
                .FirstOrDefaultAsync(x => x.IsDeleted == 0 && x.Id == id)
                ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

            return employee.ResConvert();
        }

        public async Task<ResEmployeeDTO> CreateEmployeeAsync(ReqEmployeeDTO employee)
        {
            var newEmploy = employee.MainConvert();
            _context.TEmployees.Add(newEmploy);
            await _context.SaveChangesAsync();
            return newEmploy.ResConvert();
        }

        public async Task<ResEmployeeDTO> UpdateEmployeeAsync(string id, ReqEmployeeDTO employee)
        {
            var existingEmploy = await _context.TEmployees
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

            var updatedEmployee = employee.MainConvert();
            updatedEmployee.Id = id; // Preserve original ID

            _context.Entry(existingEmploy).CurrentValues.SetValues(updatedEmployee);
            await _context.SaveChangesAsync();

            return existingEmploy.ResConvert();
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            var employee = await _context.TEmployees
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

            employee.IsDeleted = 1;
            await _context.SaveChangesAsync();
        }

        public async Task<bool> EmployeeExistsAsync(string id)
        {
            return await _context.TEmployees
                .AnyAsync(e => e.Id == id && e.IsDeleted == 0);
        }
    }
}
