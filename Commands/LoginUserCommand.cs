using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Practice.Data;
using Practice.Models;
using Practice.Services;

namespace Practice.Commands;

    public record LoginUserCommand(
        string Username,
        string Password
        ): IRequest<LoginResult>;

public class LoginResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Token { get; set; }
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
    var user =
            await _context.Users
                .FirstOrDefaultAsync(
                    x => x.Username ==
                    request.Username);

        if (user == null)
        {
            return new LoginResult
            {
                Success = false,
                Message = "empty user"
            };
        }

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return new LoginResult
            {
                Success = false,
                Message = "invalid credentials"
            };
        }

var token =
    _jwtService.GenerateToken(user);


        return new LoginResult
        {
            Success = true,
            Message = "User registered successfully",
            Token=token

        };
    }
    }

