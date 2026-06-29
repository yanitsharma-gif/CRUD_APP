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
    private readonly IConfiguration _configuration;

    public AuthController(
        AppDbContext context,
       
        IMediator mediator,
        IConfiguration configuation)
    {
        _context = context;
        
        _mediator = mediator;
        _configuration = configuation;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        Register request, CancellationToken cancellationToken)

    {
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
        if (request.secretKey == _configuration["secret"])
        {
            user.role = "admin";
        }
        else
        {
            user.role = "user";
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