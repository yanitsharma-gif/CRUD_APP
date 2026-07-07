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
    
  
    private readonly IMediator _mediator;
   

    public AuthController(
        IMediator mediator
        )
    {
       
        
        _mediator = mediator;
       
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
        var result = await _mediator.Send(
       new LoginUserCommand(
           request.Username,
           request.Password,
           request.secretKey
        ),
       cancellationToken);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);


    }
}