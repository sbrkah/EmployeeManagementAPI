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
    public class StStatusController : ControllerBase
    {
        private readonly EmanagerContext _context;

        public StStatusController(EmanagerContext context)
        {
            _context = context;
        }

        // GET: api/StStatus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StStatus>>> GetStStatuses()
        {
            return await _context.StStatuses.ToListAsync();
        }

        // GET: api/StStatus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StStatus>> GetStStatus(string id)
        {
            var stStatus = await _context.StStatuses.FindAsync(id);

            if (stStatus == null)
            {
                return NotFound();
            }

            return stStatus;
        }

        // PUT: api/StStatus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStStatus(string id, StStatus stStatus)
        {
            if (id != stStatus.Id)
            {
                return BadRequest();
            }

            _context.Entry(stStatus).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StStatusExists(id))
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

        // POST: api/StStatus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StStatus>> PostStStatus(StStatus stStatus)
        {
            _context.StStatuses.Add(stStatus);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StStatusExists(stStatus.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetStStatus", new { id = stStatus.Id }, stStatus);
        }

        // DELETE: api/StStatus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStStatus(string id)
        {
            var stStatus = await _context.StStatuses.FindAsync(id);
            if (stStatus == null)
            {
                return NotFound();
            }

            _context.StStatuses.Remove(stStatus);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StStatusExists(string id)
        {
            return _context.StStatuses.Any(e => e.Id == id);
        }
    }
}
