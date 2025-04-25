using EmployeeManagementAPI.Models;
using EmployeeManagementAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService
{
    private readonly JwtSettings _jwtSettings;
    private readonly EmanagerContext _context;

    public AuthService(IOptions<JwtSettings> jwtSettings, EmanagerContext context)
    {
        _jwtSettings = jwtSettings.Value;
        _context = context;
    }

    public async Task<ResMessageDTO> TryLogin(string username, string password)
    {
        var _acc = await _context.TAuths.Where(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();

        if(_acc != null)
        {
            return new ResMessageDTO { Message = _acc.AccessLevel ?? "User", Success = true };
        }

        return new ResMessageDTO { Success = false, Message = string.Empty };
    }

    public async Task<ResMessageDTO> CreateAdmin(string username, string password, string employeeId)
    {
        if(!await _context.TAuths.AnyAsync(x => x.Username == username))
        {
            TAuth _newAcc = new TAuth
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Password = password,
                AccessLevel = "Admin",
                EmployeeId = employeeId
            };

            _context.TAuths.Add(_newAcc);
            await _context.SaveChangesAsync();
            return new ResMessageDTO { Success = true, Message =  $"User {username} dengan akses admin berhasil dibuat!" };
        }

        return new ResMessageDTO { Success = false, Message = "Username sudah dipakai!" };
    }

    public async Task<ResMessageDTO> CreateUser(string username, string password, string employeeId)
    {
        if (!await _context.TAuths.AnyAsync(x => x.Username == username))
        {
            TAuth _newAcc = new TAuth
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                Password = password,
                AccessLevel = "User",
                EmployeeId = employeeId
            };

            _context.TAuths.Add(_newAcc);
            await _context.SaveChangesAsync();
            return new ResMessageDTO { Success = true, Message = $"User {username} dengan akses user berhasil dibuat!" };
        }

        return new ResMessageDTO { Success = false, Message = "Username sudah dipakai!" };
    }

    public string GenerateToken(string username, string? role = null)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add role if provided
        if (!string.IsNullOrEmpty(role))
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}