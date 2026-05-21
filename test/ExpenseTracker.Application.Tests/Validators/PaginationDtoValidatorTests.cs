using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators;

public class PaginationDtoValidatorTests
{
    private readonly PaginationDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_PageNumber_And_PageSize_Are_Valid()
    {
        var dto = new PaginationDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Fail_When_PageNumber_Is_Invalid(int pageNumber)
    {
        var dto = new PaginationDto
        {
            PageNumber = pageNumber,
            PageSize = 10
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.PageNumber);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Fail_When_PageSize_Is_Invalid(int pageSize)
    {
        var dto = new PaginationDto
        {
            PageNumber = 1,
            PageSize = pageSize
        };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }
}