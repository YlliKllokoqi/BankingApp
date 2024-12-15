using BankingApp.Application.DTOs;
using FluentValidation;

namespace BankingApp.Application.Validations.AuthValidations;

public class UpdateValidation : AbstractValidator<UpdateDto>
{
    public UpdateValidation()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required").Length(1, 20).WithMessage("Username must be between 2 and 20 characters");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required").Matches("^[a-zA-Z]+$") // Regular expression for alphabetic letters
            .WithMessage("Only alphabetic letters are allowed.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").Matches("^[a-zA-Z]+$") // Regular expression for alphabetic letters
            .WithMessage("Only alphabetic letters are allowed.");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.PostalCode).NotEmpty().WithMessage("PostalCode is required");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
    }
}