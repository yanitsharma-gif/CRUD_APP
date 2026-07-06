using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Practice.Services;
using Practice.Repositories;
namespace Practice.Commands;

    public record LoginUserCommand(
    string Username,
    string Password,
    string secretKey
        ) : IRequest<LoginResult>;



public class LoginResult 
{ 
    public bool   Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;


}

public class LoginUserHandler
: IRequestHandler<LoginUserCommand, LoginResult>
{
    private readonly LoginRepo _context;
    private readonly JwtService _jwtService;
    public LoginUserHandler(LoginRepo context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
       
    }
    public async Task<LoginResult> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.GetUserAsync(request);

        if (user == null)
        {
            return new LoginResult
            {
                Success = false,
                Message = "User does not exist",
                Token = ""

            };
        }
        var token =
    _jwtService.GenerateToken(user);


        return new LoginResult
        {
            Success = true,
            Message = "User login successfully",
            Token=token

        };
    }
    }

