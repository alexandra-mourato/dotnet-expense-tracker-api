using ExpenseTracker.Application.Common;
using ExpenseTracker.Application.Dtos;

namespace ExpenseTracker.Application.Tests.Common;

public class PagedResultFactoryTests
{
    public static IEnumerable<object[]> TotalPagesCases =>
        new List<object[]>
        {
            new object[] { 0, 10, 0 },
            new object[] { 1, 10, 1 },
            new object[] { 10, 10, 1 },
            new object[] { 11, 10, 2 },
            new object[] { 25, 10, 3 },
            new object[] { -1, 10, 0 },
            new object[] { 10, 0, 0 },
            new object[] { 10, -5, 0 }
        };

    [Theory]
    [MemberData(nameof(TotalPagesCases))]
    public void Create_Should_Calculate_TotalPages_Correctly(int totalItems, int pageSize, int expectedTotalPages)
    {
        var items = new List<ExpenseDto>();

        var result = PagedResultFactory.Create(items, totalItems, pageNumber: 1, pageSize: pageSize);

        Assert.Equal(expectedTotalPages, result.TotalPages);
    }

    [Fact]
    public void Create_Should_Return_Correct_Metadata_And_Items()
    {
        var items = new List<ExpenseDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Description = "A",
                Amount = 10m,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CategoryName = "Food"
            },
            new()
            {
                Id = Guid.NewGuid(),
                Description = "B",
                Amount = 20m,
                Date = DateTime.UtcNow,
                CategoryId = Guid.NewGuid(),
                CategoryName = "Transport"
            }
        };

        var result = PagedResultFactory.Create(items, totalItems: 25, pageNumber: 2, pageSize: 10);

        Assert.Equal(2, result.PageNumber);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(25, result.TotalItems);
        Assert.Equal(3, result.TotalPages);
        Assert.Equal(2, result.Items.Count());
    }
}