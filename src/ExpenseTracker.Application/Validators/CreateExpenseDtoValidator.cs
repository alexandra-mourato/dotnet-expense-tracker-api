using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Input;
using FluentValidation;

namespace ExpenseTracker.Application.Validators;

public class CreateExpenseDtoValidator : AbstractValidator<CreateExpenseDto>
{
    public CreateExpenseDtoValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Amount)
            .GreaterThan(0);

        RuleFor(x => x.Date)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}