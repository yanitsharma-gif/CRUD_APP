using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Practice.Data;
using Practice.Models;
using Practice.Services;

namespace Practice.Commands;

    public record LoginUserCommand(
        User user 
        ): IRequest<LoginResult>;



public class LoginResult 
{ 
    public bool   Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;


}

public class LoginUserHandler
: IRequestHandler<LoginUserCommand, LoginResult>
{
    private readonly AppDbContext _context;
    private readonly JwtService _jwtService;
    public LoginUserHandler(AppDbContext context, JwtService jwtService)
    {
        _context = context;
        _jwtService = jwtService;
    }
    public async Task<LoginResult> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken)
    {


        var token =
    _jwtService.GenerateToken(request.user);


        return new LoginResult
        {
            Success = true,
            Message = "User registered successfully",
            Token=token

        };
    }
    }

