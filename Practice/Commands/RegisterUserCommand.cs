using MediatR;
using Practice.Repositories;
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
    private readonly RegisterRepo _repo;

    public RegisterUserHandler(RegisterRepo repo)
    {
       
        _repo = repo;
    }

    public async Task<RegisterResult> Handle(
        RegisterUserCommand request,
        CancellationToken cancellationToken)
    {

        var user = await _repo.GetUserAsync(request);

        if (user.Success == 1)
        {
            new RegisterResult {
                Message = "User registered Succesfully",
                Success = true
            };

        }

        if (user.Success == 2)
        {
            return new RegisterResult
            {
                Message = "Database failure",
                Success = false
            };
        }

        return new RegisterResult
        {
            Message = "Username or email already exists",
            Success = false

        };
            
            }
}