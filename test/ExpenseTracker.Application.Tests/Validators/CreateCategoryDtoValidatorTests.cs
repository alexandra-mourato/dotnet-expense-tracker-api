using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators;

public class CreateCategoryDtoValidatorTests
{
    private readonly CreateCategoryDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Name_Is_Valid()
    {
        var dto = new CreateCategoryDto { Name = "Food" };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [MemberData(nameof(InvalidNames))]
    public void Should_Fail_When_Name_Is_Invalid(string name)
    {
        var dto = new CreateCategoryDto { Name = name };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
    
    public static IEnumerable<object[]> InvalidNames =>
        new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { null! }
        };
}