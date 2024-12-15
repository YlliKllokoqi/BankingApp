using BankingApp.Application.DTOs;
using FluentValidation;

namespace BankingApp.Application.Validations.AuthValidations;

public class LoginValidation : AbstractValidator<LoginDto>
{
    public LoginValidation()
    {
        RuleFor(x => x.email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.password).NotEmpty().WithMessage("Password is required");
    }
}