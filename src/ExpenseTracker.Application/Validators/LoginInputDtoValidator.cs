using ExpenseTracker.Application.Dtos.Input;
using FluentValidation;

namespace ExpenseTracker.Application.Validators;

public class LoginInputDtoValidator : AbstractValidator<LoginInputDto>
{
    public LoginInputDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty();
    }
}