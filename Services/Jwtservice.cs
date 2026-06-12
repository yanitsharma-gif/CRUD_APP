using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Practice.Models;

namespace Practice.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)// dependency injection
    {
        _configuration = configuration;
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
           
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("Email",user.Email),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("FirstName", user.FirstName),
            new Claim("LastName", user.LastName),
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}