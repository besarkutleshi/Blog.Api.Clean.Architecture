using FluentValidation;
using static Blog.Application.Features.Authentication.Commands.Login;

namespace Blog.Application.Features.Authentication.Validators;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.LoginDto.Email)
            .NotEmpty().WithMessage("Email should not be empty")
            .MaximumLength(50).WithMessage("Email cannot have more than 50 characters");

        RuleFor(x => x.LoginDto.Password)
            .NotEmpty().WithMessage("Password should not be empty")
            .MaximumLength(50).WithMessage("Password cannot have more than 50 characters");
    }
}
