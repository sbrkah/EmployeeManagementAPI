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
    public class TEmployeesController : ControllerBase
    {
        private readonly ITEmployeeService _employeeService;

        public TEmployeesController(ITEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEmployee>>> GetTEmployees()
        {
            return Ok(await _employeeService.GetAllEmployeesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TEmployee>> GetTEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTEmployee(string id, TEmployee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (!await _employeeService.EmployeeExistsAsync(id))
            {
                return NotFound();
            }

            try
            {
                await _employeeService.UpdateEmployeeAsync(id, employee);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TEmployee>> PostTEmployee(TEmployee employee)
        {
            if (await _employeeService.EmployeeExistsAsync(employee.Id))
            {
                return Conflict();
            }

            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction("GetTEmployee", new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteEmployeeAsync(id);
            return NoContent();
        }
    }
}