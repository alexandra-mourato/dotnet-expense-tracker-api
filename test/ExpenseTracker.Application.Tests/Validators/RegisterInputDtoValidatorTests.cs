using ExpenseTracker.Application.Dtos.Input;
using ExpenseTracker.Application.Validators;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Application.Tests.Validators;

public class RegisterInputDtoValidatorTests
{
    private readonly RegisterInputDtoValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData("invalid-email")]
    public void Should_Have_Error_When_Email_Is_Invalid(string email)
    {
        // Arrange
        var dto = new RegisterInputDto
        {
            Email = email,
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Too_Short()
    {
        // Arrange
        var dto = new RegisterInputDto
        {
            Email = "test@test.com",
            Password = "123"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        // Arrange
        var dto = new RegisterInputDto
        {
            Email = "test@test.com",
            Password = "Password123"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }
}