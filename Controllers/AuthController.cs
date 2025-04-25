using Microsoft.AspNetCore.Mvc;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Login(ReqLoginDTO request)
        {
            var result = await _authService.TryLogin(request.Username, request.Password);

            if (!result.Success)
            {
                return Unauthorized("Username atau password salah");
            }

            var token = _authService.GenerateToken(request.Username, result.Message ?? "User");
            return Ok(token);
        }

        [HttpPost("createAdmin")]
        [Authorize(Roles = "SuperUser")]
        public async Task<IActionResult> CreateAdmin(ReqCredentialDTO request)
        {
            var result = await _authService.CreateAdmin(request.Username, request.Password, request.EmployeeId);

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("createUser")]
        [Authorize(Roles = "Admin,SuperUser")]
        public async Task<IActionResult> CreateUser(ReqCredentialDTO request)
        {
            var result = await _authService.CreateUser(request.Username, request.Password, request.EmployeeId);

            if (!result.Success)
            {
                throw new Exception(result.Message);
            }

            return Ok(result);
        }
    }
}
