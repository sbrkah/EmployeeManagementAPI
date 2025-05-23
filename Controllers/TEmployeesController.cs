﻿using System.Collections.Generic;
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
    public class TEmployeesController : ControllerBase
    {
        private readonly ITEmployeeService _employeeService;

        public TEmployeesController(ITEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ResEmployeeDTO>>> GetTEmployees()
        {
            return Ok(await _employeeService.GetAllEmployeesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResEmployeeDTO>> GetTEmployee(string id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return employee;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<IActionResult> PutTEmployee(string id, ReqEmployeeDTO employee)
        {
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
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<ActionResult<ResEmployeeDTO>> PostTEmployee(ReqEmployeeDTO employee)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction("GetTEmployee", new { id = createdEmployee.Id }, createdEmployee);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,SuperUser")]
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