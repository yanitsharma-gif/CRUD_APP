using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Practice.Data;
using Practice.Models;
using Practice.Services;



namespace Practice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;

    public AuthController(
        AppDbContext context,
        JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        Register request)
    {
        if (request.Email == null) return BadRequest("email is not defined");
        string email = request.Email;

        bool isValid1 = Regex.IsMatch(
            email,
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");

        if (!isValid1)
        {
            return BadRequest("Invalid email format.");
        }
        if (request.Username == null) return BadRequest("username is not defined");

        var existingUser =
            await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username ==
                    request.Username);

        if (existingUser != null)
        {
            return BadRequest(
                "Username already exists");
        }
        string password = request.Password;

        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        bool isValid = Regex.IsMatch(password, pattern);

        if (!isValid)
        {
            return BadRequest(
                "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character.");
        }

        var user = new User
        {
            Email=email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password)
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok("User Registered");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        Login request)


    {
        if (request.Username == null) return BadRequest("username is not defined");
        var user =
            await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username ==
                    request.Username);

        if (user == null)
        {
            return Unauthorized("invalid credentials");
        }

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return Unauthorized("invalid credentials ");
        }

        var token =
            _jwtService.GenerateToken(user);

        return Ok(
            new 
            {
                AccessToken = token,
                User = new
                {
                    user.Email,
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Username
                }
            });
    }
}