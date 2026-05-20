using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ExpenseTracker.Api.Filters;

public class ValidationFilter<T> : IAsyncActionFilter
    where T : class
{
    private readonly IValidator<T> _validator;

    public ValidationFilter(IValidator<T> validator)
    {
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var argument = context.ActionArguments
            .Values
            .OfType<T>()
            .FirstOrDefault();

        if (argument is null)
        {
            await next();
            return;
        }

        var validationResult = await _validator.ValidateAsync(argument);

        if (!validationResult.IsValid)
        {
            context.Result = new BadRequestObjectResult(
                validationResult.Errors);

            return;
        }

        await next();
    }
}