using MediatR;
using Practice.Data;
using Practice.Models;

namespace Practice.Commands;

public record RegisterUserCommand(
 
    string FirstName,
    string LastName,
    string Email,
    string Username,
    string Password,
    string Address
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
                   request.Password),
               Address = request.Address,
               
        };

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        catch(Exception error)
        {
            var innerMessage = error.InnerException?.Message ?? "";

            if (innerMessage.Contains("IX_Users_Email"))
                return new RegisterResult { Success = false, Message = "Email already exists" };

            if (innerMessage.Contains("IX_Users_Username"))
                return new RegisterResult { Success = false, Message = "username already taken" };

            if (innerMessage.Contains("IX_Users_Address"))
                return new RegisterResult { Success = false, Message = "Address already exists" };

            return new RegisterResult { Success = false, Message = "Registration failed" };
        }
      

        

        return new RegisterResult
        {
            Success = true,
            Message = "User registered successfully"
        };
    }
}