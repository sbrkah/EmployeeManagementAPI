using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagementAPI.Models;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StClassesController : ControllerBase
    {
        private readonly EmanagerContext _context;

        public StClassesController(EmanagerContext context)
        {
            _context = context;
        }

        // GET: api/StClasses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StClass>>> GetStClasses()
        {
            return await _context.StClasses.ToListAsync();
        }

        // GET: api/StClasses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StClass>> GetStClass(string id)
        {
            var stClass = await _context.StClasses.FindAsync(id);

            if (stClass == null)
            {
                return NotFound();
            }

            return stClass;
        }

        // PUT: api/StClasses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStClass(string id, StClass stClass)
        {
            if (id != stClass.Id)
            {
                return BadRequest();
            }

            _context.Entry(stClass).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StClassExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StClasses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StClass>> PostStClass(StClass stClass)
        {
            _context.StClasses.Add(stClass);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StClassExists(stClass.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStClass", new { id = stClass.Id }, stClass);
        }

        // DELETE: api/StClasses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStClass(string id)
        {
            var stClass = await _context.StClasses.FindAsync(id);
            if (stClass == null)
            {
                return NotFound();
            }

            _context.StClasses.Remove(stClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StClassExists(string id)
        {
            return _context.StClasses.Any(e => e.Id == id);
        }
    }
}
