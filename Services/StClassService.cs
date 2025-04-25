using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

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
        private readonly EmanagerContext _context;

        public StClassService(EmanagerContext context)
        {
            _context = context;
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
            var stClass = await _context.StClasses
                .FirstOrDefaultAsync(x => x.IsDeleted == 0 && x.Id == id)
                ?? throw new KeyNotFoundException($"Class with ID {id} not found");

            return stClass.ResConvert();
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
        }

        public async Task<bool> ClassExistsAsync(string id)
        {
            return await _context.StClasses
                .AnyAsync(e => e.Id == id && e.IsDeleted == 0);
        }
    }
}