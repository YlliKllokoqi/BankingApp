using BankingApp.Application.DTOs;
using FluentValidation;

namespace BankingApp.Application.Validations.AuthValidations;

public class RegisterValidation : AbstractValidator<RegisterDto>
{
    public RegisterValidation()
    {
        RuleFor(x => x.UserName).NotEmpty().WithMessage("Username is required").Length(1, 20).WithMessage("Username must be between 2 and 20 characters");
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("First name is required").Matches("^[a-zA-Z]+$") // Regular expression for alphabetic letters
            .WithMessage("Only alphabetic letters are allowed.");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("Last name is required").Matches("^[a-zA-Z]+$") // Regular expression for alphabetic letters
            .WithMessage("Only alphabetic letters are allowed.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .Matches("^[a-zA-Z0-9!@#$%^&*()_+=-]+$").WithMessage("Password contains invalid characters.");
        RuleFor(x => x.DateOfBirth).NotEmpty().WithMessage("Date of birth is required").Must(ageValidator).WithMessage("Must be at least 18 years of age");
        RuleFor(x => x.Gender).IsInEnum().WithMessage("Gender must be either M or F");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid Email format");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required");
        RuleFor(x => x.PostalCode).NotEmpty().WithMessage("PostalCode is required");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required");
    }

    private bool ageValidator(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var eighteenYearsago = today.AddYears(-18);
        return dateOfBirth <= eighteenYearsago;
    }
}