using Microsoft.AspNetCore.Mvc;
using EmployeeManagementAPI.Models.DTO;

namespace EmployeeManagementAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] ReqLoginDTO request)
        {
            // Contoh validasi sederhana (ganti dengan logic database)
            if (request.Username == "admin" && request.Password == "password")
            {
                var token = _authService.GenerateToken(request.Username, "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized("Username atau password salah");
        }
    }
}
