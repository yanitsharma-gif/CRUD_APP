using FluentValidation;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Practice.Data;
using Practice.DTO;

public class LoginRequestValidator : AbstractValidator<Login>
{
    private readonly AppDbContext _context;
    public LoginRequestValidator(AppDbContext context)
    {
        _context = context;
        RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Please enter Username.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("please enter password");
    }
}