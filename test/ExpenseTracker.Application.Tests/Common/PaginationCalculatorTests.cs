using ExpenseTracker.Application.Common;

namespace ExpenseTracker.Application.Tests.Common;

public class PaginationCalculatorTests
{
    [Theory]
    [InlineData(1, 10, 0)]
    [InlineData(2, 10, 10)]
    [InlineData(3, 25, 50)]
    [InlineData(0, 10, 0)]
    [InlineData(-1, 10, 0)]
    [InlineData(2, 0, 0)]
    [InlineData(2, -5, 0)]
    public void CalculateSkip_ReturnsExpectedValue(int pageNumber, int pageSize, int expectedSkip)
    {
        // Act
        var result = PaginationCalculator.CalculateSkip(pageNumber, pageSize);

        // Assert
        Assert.Equal(expectedSkip, result);
    }

    [Theory]
    [InlineData(0, 10, 0)]
    [InlineData(1, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(11, 10, 2)]
    [InlineData(25, 10, 3)]
    [InlineData(-1, 10, 0)]
    [InlineData(10, 0, 0)]
    [InlineData(10, -5, 0)]
    public void CalculateTotalPages_ReturnsExpectedValue(int totalItems, int pageSize, int expectedTotalPages)
    {
        // Act
        var result = PaginationCalculator.CalculateTotalPages(totalItems, pageSize);

        // Assert
        Assert.Equal(expectedTotalPages, result);
    }
}