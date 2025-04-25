using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace EmployeeManagementAPI.Services
{
    public interface IStClassService
    {
        Task<IEnumerable<ResClassDTO>> GetAllClassesAsync();
        Task<ResClassDTO> GetClassByIdAsync(string id);
        Task<ResClassDTO> CreateClassAsync(ReqClassDTO stClass);
        Task<ResClassDTO> UpdateClassAsync(string id, ReqClassDTO stClass);
        Task DeleteClassAsync(string id);
        Task<bool> ClassExistsAsync(string id);
    }

    public class StClassService : IStClassService
    {
        private readonly IDistributedCache _cache;
        private readonly EmanagerContext _context;
        private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(15);

        public StClassService(EmanagerContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async void RemoveCache(string id)
        {
            if (await _cache.GetStringAsync($"Class_{id}") != null)
            {
                await _cache.RemoveAsync($"Class_{id}");
            }
        }

        public async Task<IEnumerable<ResClassDTO>> GetAllClassesAsync()
        {
            var classes = await _context.StClasses
                .Where(x => x.IsDeleted == 0)
                .ToListAsync();

            return classes.Select(x => x.ResConvert());
        }

        public async Task<ResClassDTO> GetClassByIdAsync(string id)
        {
            string cacheKey = $"Class_{id}";
            var cachedItem = await _cache.GetStringAsync(cacheKey);
            if(cachedItem != null)
            {
                return JsonConvert.DeserializeObject<ResClassDTO>(cachedItem);
            }

            var stClass = await _context.StClasses
                .FirstOrDefaultAsync(x => x.IsDeleted == 0 && x.Id == id)
                ?? throw new KeyNotFoundException($"Class with ID {id} not found");

            ResClassDTO resCLass = stClass.ResConvert();

            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(resCLass),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = _cacheExpiry }
            );

            return resCLass;
        }

        public async Task<ResClassDTO> CreateClassAsync(ReqClassDTO reqClass)
        {
            var newClass = reqClass.MainConvert();
            _context.StClasses.Add(newClass);
            await _context.SaveChangesAsync();
            return newClass.ResConvert();
        }

        public async Task<ResClassDTO> UpdateClassAsync(string id, ReqClassDTO reqClass)
        {
            var existingClass = await _context.StClasses
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Class with ID {id} not found");

            var updatedClass = reqClass.MainConvert();
            updatedClass.Id = id; // Preserve original ID
            RemoveCache(id);

            _context.Entry(existingClass).CurrentValues.SetValues(updatedClass);
            await _context.SaveChangesAsync();

            return existingClass.ResConvert();
        }

        public async Task DeleteClassAsync(string id)
        {
            var stClass = await _context.StClasses
                .FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == 0)
                ?? throw new KeyNotFoundException($"Class with ID {id} not found");

            stClass.IsDeleted = 1;
            await _context.SaveChangesAsync();
            RemoveCache(id);

            List<TEmployee> employees = await _context.TEmployees.Where(x => x.Class == id).ToListAsync();

            foreach(TEmployee emp in employees)
            {
                emp.Class = string.Empty;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ClassExistsAsync(string id)
        {
            return await _context.StClasses
                .AnyAsync(e => e.Id == id && e.IsDeleted == 0);
        }
    }
}