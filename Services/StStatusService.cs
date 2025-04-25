using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

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

        public StStatusService(EmanagerContext context)
        {
            _context = context;
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
            var stats = await _context.StStatuses
                    .FirstOrDefaultAsync(x => x.IsDeleted == 0 && x.Id == id)
                    ?? throw new KeyNotFoundException($"Status with ID {id} not found");

            return stats.ResConvert();
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
        }

        public async Task<bool> StatusExistsAsync(string id)
        {
            return await _context.StStatuses
                .AnyAsync(e => e.Id == id && e.IsDeleted == 0);
        }
    }
}