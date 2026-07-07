using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Practice.Configurations;
using Practice.Models;

namespace Practice.Services;

public class JwtService
{
    private readonly JwtSettings _jwtSettings;

    public JwtService(IOptions<JwtSettings> options)// dependency injection
    {
        _jwtSettings = options.Value;
    }
    
    public string GenerateToken(User user)
    {
        // --- Username Validation Step ---
        if (string.IsNullOrWhiteSpace(user.Username))
        {
            throw new ArgumentException("Username cannot be empty", nameof(user.Username));
        }
        // --- Password Validation Step ---
        if (string.IsNullOrWhiteSpace(user.PasswordHash))
        {
            throw new ArgumentException("Password should not be empty", nameof(user.PasswordHash));
        }


        var claims = new[]
        {

            new Claim(ClaimTypes.NameIdentifier, user.id.ToString()),
            new Claim("Email",user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
            new Claim(ClaimTypes.Role,user.role)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _jwtSettings.Key));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);
        
        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}