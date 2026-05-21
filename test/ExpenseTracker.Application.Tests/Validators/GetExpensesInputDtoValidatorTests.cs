using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators;

public class GetExpensesInputDtoValidatorTests
{
    private readonly GetExpensesInputDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Input_Is_Valid()
    {
        var dto = new GetExpensesInputDto
        {
            PageNumber = 1,
            PageSize = 10,
            Month = 5,
            Year = 2026
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(13)]
    public void Should_Fail_When_Month_Is_Invalid(int month)
    {
        var dto = new GetExpensesInputDto
        {
            PageNumber = 1,
            PageSize = 10,
            Month = month
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Month);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-2020)]
    public void Should_Fail_When_Year_Is_Invalid(int year)
    {
        var dto = new GetExpensesInputDto
        {
            PageNumber = 1,
            PageSize = 10,
            Year = year
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Year);
    }
    
}