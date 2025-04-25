using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services
{
    public interface ITEmployeeService
    {
        Task<IEnumerable<TEmployee>> GetAllEmployeesAsync();
        Task<TEmployee> GetEmployeeByIdAsync(string id);
        Task<TEmployee> CreateEmployeeAsync(TEmployee employee);
        Task UpdateEmployeeAsync(string id, TEmployee employee);
        Task DeleteEmployeeAsync(string id);
        Task<bool> EmployeeExistsAsync(string id);
    }

    public class TEmployeeService
    {
        private readonly EmanagerContext _context;

        public TEmployeeService(EmanagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEmployee>> GetAllEmployeesAsync()
        {
            return await _context.TEmployees.ToListAsync();
        }

        public async Task<TEmployee> GetEmployeeByIdAsync(string id)
        {
            return await _context.TEmployees.FindAsync(id);
        }

        public async Task<TEmployee> CreateEmployeeAsync(TEmployee employee)
        {
            _context.TEmployees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task UpdateEmployeeAsync(string id, TEmployee employee)
        {
            _context.Entry(employee).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            var employee = await _context.TEmployees.FindAsync(id);
            if (employee != null)
            {
                _context.TEmployees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> EmployeeExistsAsync(string id)
        {
            return await _context.TEmployees.AnyAsync(e => e.Id == id);
        }
    }
}
