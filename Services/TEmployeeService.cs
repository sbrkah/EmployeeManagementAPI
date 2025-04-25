using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

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
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(15);
        private readonly ILogger<TEmployeeService> _logger;

        public TEmployeeService(EmanagerContext context, IDistributedCache cache, ILogger<TEmployeeService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        public async void RemoveCache(string id)
        {
            if (await _cache.GetStringAsync($"Emp_{id}") != null)
            {
                await _cache.RemoveAsync($"Emp_{id}");
            }
        }

        public async Task<IEnumerable<ResEmployeeDTO>> GetAllEmployeesAsync()
        {
            var employees = await _context.TEmployees
                .Where(x => x.IsDeleted == 0)
                .ToListAsync();

            _logger.LogInformation($"Mengambil data employee..");
            return employees.Select(x => x.ResConvert());
        }

        public async Task<ResEmployeeDTO> GetEmployeeByIdAsync(string id)
        {
            string cacheKey = $"Emp_{id}";
            var cachedItem = await _cache.GetStringAsync(cacheKey);
            if (cachedItem != null)
            {
                return JsonConvert.DeserializeObject<ResEmployeeDTO>(cachedItem);
            }

            var employee = await _context.TEmployees
                .FirstOrDefaultAsync(x => x.IsDeleted == 0 && x.Id == id)
                ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

            ResEmployeeDTO resEmployee = employee.ResConvert();

            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(resEmployee),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiry }
            );

            return resEmployee;
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
            RemoveCache(id);

            _context.Entry(existingEmploy).CurrentValues.SetValues(updatedEmployee);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Data record employee {id} telah diperbarui..");

            return existingEmploy.ResConvert();
        }

        public async Task DeleteEmployeeAsync(string id)
        {
            var employee = await _context.TEmployees
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Employee with ID {id} not found");

            employee.IsDeleted = 1;
            RemoveCache(id);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Menghapus employee {id} dari record..");
        }

        public async Task<bool> EmployeeExistsAsync(string id)
        {
            return await _context.TEmployees
                .AnyAsync(e => e.Id == id && e.IsDeleted == 0);
        }
    }
}
