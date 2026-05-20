using ExpenseTracker.Application.Dtos.Input;
using FluentValidation;

namespace ExpenseTracker.Application.Validators;

public class GetExpensesInputDtoValidator : AbstractValidator<GetExpensesInputDto>
{
    public GetExpensesInputDtoValidator()
    {
        Include(new PaginationDtoValidator());

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .When(x => x.Month.HasValue);

        RuleFor(x => x.Year)
            .InclusiveBetween(2000, 2100)
            .When(x => x.Year.HasValue);
    }
}