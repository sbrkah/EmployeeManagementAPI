using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services
{
    public interface IStStatusService
    {
        Task<IEnumerable<StStatus>> GetAllStatusesAsync();
        Task<StStatus> GetStatusByIdAsync(string id);
        Task<StStatus> CreateStatusAsync(StStatus stStatus);
        Task UpdateStatusAsync(string id, StStatus stStatus);
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

        public async Task<IEnumerable<StStatus>> GetAllStatusesAsync()
        {
            return await _context.StStatuses.ToListAsync();
        }

        public async Task<StStatus> GetStatusByIdAsync(string id)
        {
            return await _context.StStatuses.FindAsync(id);
        }

        public async Task<StStatus> CreateStatusAsync(StStatus stStatus)
        {
            _context.StStatuses.Add(stStatus);
            await _context.SaveChangesAsync();
            return stStatus;
        }

        public async Task UpdateStatusAsync(string id, StStatus stStatus)
        {
            _context.Entry(stStatus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteStatusAsync(string id)
        {
            var stStatus = await _context.StStatuses.FindAsync(id);
            if (stStatus != null)
            {
                _context.StStatuses.Remove(stStatus);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> StatusExistsAsync(string id)
        {
            return await _context.StStatuses.AnyAsync(e => e.Id == id);
        }
    }
}