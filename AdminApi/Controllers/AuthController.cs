using System.Security.Claims;
using AdminApi.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController(
    IOptions<AdminAuthOptions> adminOpt,
    ITokenService tokenService) : ControllerBase
{
    public sealed record LoginRequest(string Username, string Password);
    public sealed record LoginResponse(string AccessToken, DateTime ExpiresAtUtc, 
        string Username, string Role);
    
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<LoginResponse> GetToken([FromBody] LoginRequest req)
    {
        var a = adminOpt.Value;

        // Constant-time-ish comparison (simple version for assignment)
        if (!string.Equals(req.Username, a.Username, StringComparison.Ordinal) ||
            !string.Equals(req.Password, a.Password, StringComparison.Ordinal))
        {
            return Unauthorized(new { message = "Invalid username or password" });
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, a.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var token = tokenService.CreateToken(claims);
        var expires = DateTime.UtcNow.AddMinutes(60); // keep in sync with JwtOptions

        return Ok(new LoginResponse(token, expires, a.Username, "Admin"));
    }
}