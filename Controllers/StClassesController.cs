using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using EmployeeManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StClassesController : ControllerBase
    {
        private readonly IStClassService _stClassService;

        public StClassesController(IStClassService stClassService)
        {
            _stClassService = stClassService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResClassDTO>>> GetStClasses()
        {
            return Ok(await _stClassService.GetAllClassesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResClassDTO>> GetStClass(string id)
        {
            var stClass = await _stClassService.GetClassByIdAsync(id);
            if (stClass == null)
            {
                return NotFound();
            }
            return stClass;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStClass(string id, ReqClassDTO stClass)
        {
            if (!await _stClassService.ClassExistsAsync(id))
            {
                return NotFound();
            }

            try
            {
                await _stClassService.UpdateClassAsync(id, stClass);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ResClassDTO>> PostStClass(ReqClassDTO stClass)
        {
            var createdClass = await _stClassService.CreateClassAsync(stClass);
            return CreatedAtAction("GetStClass", new { id = createdClass.Id }, createdClass);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStClass(string id)
        {
            var stClass = await _stClassService.GetClassByIdAsync(id);
            if (stClass == null)
            {
                return NotFound();
            }

            await _stClassService.DeleteClassAsync(id);
            return NoContent();
        }
    }
}