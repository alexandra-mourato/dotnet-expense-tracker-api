using ExpenseTracker.Application.Dtos.Input;
using FluentValidation;

namespace ExpenseTracker.Application.Validators;

public class RegisterInputDtoValidator : AbstractValidator<RegisterInputDto>
{
    public RegisterInputDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .MinimumLength(6);
    }
}