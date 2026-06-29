using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.DTO;
using Practice.Models;
namespace Practice.Validators
{
    public class RegisterRequestValidator : AbstractValidator<Register>
    {
        private readonly AppDbContext _context;

        public RegisterRequestValidator(AppDbContext context)
        {

            _context = context;
            RuleFor(x => x.Username)
                 .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage(" please enter the username ")
                .MinimumLength(3)
                .MaximumLength(50)
                .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("Username can only contain letters, numbers, and underscores.")
                .MustAsync(BeUniqueUsername)
                .WithMessage("username already exists");

            RuleFor(x => x.Email)
                 .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("email is not of supported format");

            RuleFor(x => x.Password)
                 .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .MinimumLength(8)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
                .WithMessage("password is not supported");

            RuleFor(x => x.Address)
                 .Cascade(CascadeMode.Stop)
               .NotEmpty()
               .WithMessage("Address is required");

        }
        private async Task<bool> BeUniqueUsername(
        string username,
        CancellationToken cancellationToken)
        {
            return !await _context.Users
                .AnyAsync(x => x.Username == username, cancellationToken);
        }
    }
}
