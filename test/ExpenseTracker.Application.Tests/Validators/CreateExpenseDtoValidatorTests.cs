using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators;

public class CreateExpenseDtoValidatorTests
{
    private readonly CreateExpenseDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Dto_Is_Valid()
    {
        var dto = new CreateExpenseInputDto
        {
            Description = "Lunch",
            Amount = 12.50m,
            Date = DateTime.UtcNow,
            CategoryId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Fail_When_Description_Is_Invalid(string description)
    {
        var dto = new CreateExpenseInputDto
        {
            Description = description,
            Amount = 10m,
            Date = DateTime.UtcNow,
            CategoryId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Should_Fail_When_Amount_Is_Less_Than_Or_Equal_To_Zero(decimal amount)
    {
        var dto = new CreateExpenseInputDto
        {
            Description = "Coffee",
            Amount = amount,
            Date = DateTime.UtcNow,
            CategoryId = Guid.NewGuid()
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_Fail_When_CategoryId_Is_Empty()
    {
        var dto = new CreateExpenseInputDto
        {
            Description = "Coffee",
            Amount = 2m,
            Date = DateTime.UtcNow,
            CategoryId = Guid.Empty
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.CategoryId);
    }
}