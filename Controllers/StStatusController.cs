using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StStatusController : ControllerBase
    {
        private readonly IStStatusService _stStatusService;

        public StStatusController(IStStatusService stStatusService)
        {
            _stStatusService = stStatusService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StStatus>>> GetStStatuses()
        {
            return Ok(await _stStatusService.GetAllStatusesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StStatus>> GetStStatus(string id)
        {
            var stStatus = await _stStatusService.GetStatusByIdAsync(id);
            if (stStatus == null)
            {
                return NotFound();
            }
            return stStatus;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStStatus(string id, StStatus stStatus)
        {
            if (id != stStatus.Id)
            {
                return BadRequest();
            }

            if (!await _stStatusService.StatusExistsAsync(id))
            {
                return NotFound();
            }

            try
            {
                await _stStatusService.UpdateStatusAsync(id, stStatus);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<StStatus>> PostStStatus(StStatus stStatus)
        {
            if (await _stStatusService.StatusExistsAsync(stStatus.Id))
            {
                return Conflict();
            }

            var createdStatus = await _stStatusService.CreateStatusAsync(stStatus);
            return CreatedAtAction("GetStStatus", new { id = createdStatus.Id }, createdStatus);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStStatus(string id)
        {
            var stStatus = await _stStatusService.GetStatusByIdAsync(id);
            if (stStatus == null)
            {
                return NotFound();
            }

            await _stStatusService.DeleteStatusAsync(id);
            return NoContent();
        }
    }
}