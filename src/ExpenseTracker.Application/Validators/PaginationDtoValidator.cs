using ExpenseTracker.Application.Dtos.Input;
using FluentValidation;

namespace ExpenseTracker.Application.Validators;

public class PaginationDtoValidator : AbstractValidator<PaginationDto>
{
    public PaginationDtoValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);
    }
}