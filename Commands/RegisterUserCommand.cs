using MediatR;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.Models;

namespace Practice.Commands;

public record RegisterUserCommand(
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password
) : IRequest<RegisterResult>;

public class RegisterResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}

public class RegisterUserHandler
    : IRequestHandler<RegisterUserCommand, RegisterResult>
{
    private readonly AppDbContext _context;

    public RegisterUserHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RegisterResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        var user = new User
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            Username = request.Username,
            PasswordHash =
               BCrypt.Net.BCrypt.HashPassword(
                   request.Password)
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        

        return new RegisterResult
        {
            Success = true,
            Message = "User registered successfully"
        };
    }
}