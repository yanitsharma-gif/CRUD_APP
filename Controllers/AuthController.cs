using System.Text.RegularExpressions;
using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Practice.Commands;
using Practice.Data;
using Practice.Models;
using Practice.Services;
using MediatR;


namespace Practice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;
    private readonly IMediator _mediator;

    public AuthController(
        AppDbContext context,
        JwtService jwtService,
        IMediator mediator)
    {
        _context = context;
        _jwtService = jwtService;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        Register request, CancellationToken cancellationToken)
    {
        if (request.Email == null) return BadRequest(new {
            message="email should not be null",
            errorcode=404
        });
        string email = request.Email;

        bool isValid1 = Regex.IsMatch(
            email,
            @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");

        if (!isValid1)
        {
            return BadRequest(new
            {
                Message = "email is not valid",
                ErrorCode= 404
            });
        }
        if (request.Username == null) return  BadRequest(new
        {
            Message = "username should not be null",
            ErrorCode = 404
        });

        var existingUser =
            await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username ==
                    request.Username);

        if (existingUser != null)
        {
            return BadRequest(new{
                message="username already exists",
                errorcode=403
            });
        }
        string password = request.Password;

        string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";

        bool isValid = Regex.IsMatch(password, pattern);

        if (!isValid)
        {
            return BadRequest(new {
                message= "Password must contain at least 8 characters, one uppercase letter, one lowercase letter, one number, and one special character.",
                errorcode=403
            }

                );
        }

        var result = await _mediator.Send(
        new RegisterUserCommand(
           request.FirstName,
           request.LastName,
           request.Email,
           request.Username,
           request.Password),
        cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        Login request)


    {
        if (request.Username == null) return BadRequest(new{
            message="user should not be null",
            status=404
        });
        var user =
            await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username ==
                    request.Username);

        if (user == null)
        {
            return Unauthorized(new{
                message="invalid credentials",
                status =404

            });
        }

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return Unauthorized(new{
                message="invalid credentials ",
                status=404
            });
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