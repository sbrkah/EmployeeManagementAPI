using EmployeeManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Services
{
    public interface IStClassService
    {
        Task<IEnumerable<StClass>> GetAllClassesAsync();
        Task<StClass> GetClassByIdAsync(string id);
        Task<StClass> CreateClassAsync(StClass stClass);
        Task UpdateClassAsync(string id, StClass stClass);
        Task DeleteClassAsync(string id);
        Task<bool> ClassExistsAsync(string id);
    }

    public class StClassService : IStClassService
    {
        private readonly EmanagerContext _context;
        public StClassService(EmanagerContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StClass>> GetAllClassesAsync()
        {
            return await _context.StClasses.ToListAsync();
        }

        public async Task<StClass> GetClassByIdAsync(string id)
        {
            return await _context.StClasses.FindAsync(id);
        }

        public async Task<StClass> CreateClassAsync(StClass stClass)
        {
            _context.StClasses.Add(stClass);
            await _context.SaveChangesAsync();
            return stClass;
        }

        public async Task UpdateClassAsync(string id, StClass stClass)
        {
            _context.Entry(stClass).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClassAsync(string id)
        {
            var stClass = await _context.StClasses.FindAsync(id);
            if (stClass != null)
            {
                _context.StClasses.Remove(stClass);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ClassExistsAsync(string id)
        {
            return await _context.StClasses.AnyAsync(e => e.Id == id);
        }
    }
}
