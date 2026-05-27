namespace ExpenseTracker.Application.Dtos.Output;

public class DashboardOutputDto
{
    public int Month { get; set; }

    public int Year { get; set; }

    public decimal TotalSpent { get; set; }

    public IEnumerable<TotalByCategoryDto> TotalByCategory { get; set; } = [];

    public IEnumerable<ExpenseDto> LatestExpenses { get; set; } = [];
}