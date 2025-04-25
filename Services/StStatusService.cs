using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmployeeManagementAPI.Services
{
    public interface IStStatusService
    {
        Task<IEnumerable<ResStatusDTO>> GetAllStatusesAsync();
        Task<ResStatusDTO> GetStatusByIdAsync(string id);
        Task<ResStatusDTO> CreateStatusAsync(ReqStatusDTO stStatus);
        Task<ResStatusDTO> UpdateStatusAsync(string id, ReqStatusDTO stStatus);
        Task DeleteStatusAsync(string id);
        Task<bool> StatusExistsAsync(string id);
    }

    public class StStatusService : IStStatusService
    {
        private readonly EmanagerContext _context;
        private readonly IDistributedCache _cache;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(15);

        public StStatusService(EmanagerContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async void RemoveCache(string id)
        {
            if (await _cache.GetStringAsync($"Status_{id}") != null)
            {
                await _cache.RemoveAsync($"Status_{id}");
            }
        }

        public async Task<IEnumerable<ResStatusDTO>> GetAllStatusesAsync()
        {
            var stats = await _context.StStatuses
                .Where(x => x.IsDeleted == 0)
                .ToListAsync();

            return stats.Select(x => x.ResConvert());
        }

        public async Task<ResStatusDTO> GetStatusByIdAsync(string id)
        {
            string cacheKey = $"Status_{id}";
            var cachedItem = await _cache.GetStringAsync(cacheKey);
            if (cachedItem != null)
            {
                return JsonConvert.DeserializeObject<ResStatusDTO>(cachedItem);
            }

            var stats = await _context.StStatuses
                    .FirstOrDefaultAsync(x => x.IsDeleted == 0 && x.Id == id)
                    ?? throw new KeyNotFoundException($"Status with ID {id} not found");

            ResStatusDTO resStats = stats.ResConvert();

            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(resStats),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiry }
            );

            return resStats;
        }

        public async Task<ResStatusDTO> CreateStatusAsync(ReqStatusDTO reqStatus)
        {
            var newStats = reqStatus.MainConvert();
            _context.StStatuses.Add(newStats);
            await _context.SaveChangesAsync();
            return newStats.ResConvert();
        }

        public async Task<ResStatusDTO> UpdateStatusAsync(string id, ReqStatusDTO reqStatus)
        {
            var existingStats = await _context.StStatuses
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Status with ID {id} not found");

            var updatedStatus = reqStatus.MainConvert();
            updatedStatus.Id = id; // Preserve original ID
            RemoveCache(id);

            _context.Entry(existingStats).CurrentValues.SetValues(updatedStatus);
            await _context.SaveChangesAsync();

            return existingStats.ResConvert();
        }

        public async Task DeleteStatusAsync(string id)
        {
            var stStats = await _context.StStatuses
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Status with ID {id} not found");

            stStats.IsDeleted = 1;
            await _context.SaveChangesAsync();
            RemoveCache(id);

            // Change Status Data on Employee
            List<TEmployee> employees = await _context.TEmployees.Where(x => x.Status == id).ToListAsync();

            foreach (TEmployee emp in employees)
            {
                emp.Status = string.Empty;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> StatusExistsAsync(string id)
        {
            return await _context.StStatuses
                .AnyAsync(e => e.Id == id && e.IsDeleted == 0);
        }
    }
}