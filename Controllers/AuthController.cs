using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Practice.Commands;
using Practice.Data;

using MediatR;
using Practice.DTO;


namespace Practice.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
  
    private readonly IMediator _mediator;

    public AuthController(
        AppDbContext context,
       
        IMediator mediator)
    {
        _context = context;
        
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
           request.Password,
           request.Address), 
        cancellationToken);

        

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        Login request,CancellationToken cancellationToken)


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
            return BadRequest(new
            {
                Success = false,
                Message = "empty user"
            });
           
        }

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return BadRequest(new
            {
                Success = false,
                Message = "invalid credentials"
            });
        }
        var result = await _mediator.Send(
       new LoginUserCommand(
        user),
       cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);


    }
}