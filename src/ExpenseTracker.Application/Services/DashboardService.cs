using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Dtos.Output;
using ExpenseTracker.Application.Interfaces.Persistence;
using ExpenseTracker.Application.Interfaces.Services;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Services;

public class DashboardService : IDashboardService
{
    private readonly IExpenseRepository _expenseRepository;

    public DashboardService(IExpenseRepository expenseRepository)
    {
        _expenseRepository = expenseRepository;
    }

    public async Task<DashboardOutputDto> GetMonthlyDashboardAsync(int month, int year)
    {
        var totalSpent = await _expenseRepository.GetTotalSpentAsync(month, year);

        var totalByCategory = await _expenseRepository.GetTotalByCategoryAsync(
            month,
            year);

        var latestExpenses = await _expenseRepository.GetLatestAsync(
            month,
            year,
            5);

        return new DashboardOutputDto
        {
            Month = month,
            Year = year,
            TotalSpent = totalSpent,
            TotalByCategory = totalByCategory.Select(x => new TotalByCategoryDto
            {
                Category = x.Category,
                Total = x.Total
            }),
            LatestExpenses = latestExpenses.Select(MapToExpenseDto)
        };
    }

    private static ExpenseDto MapToExpenseDto(Expense expense)
    {
        return new ExpenseDto
        {
            Id = expense.Id,
            Description = expense.Description,
            Amount = expense.Amount,
            Date = expense.Date,
            CategoryId = expense.CategoryId,
            CategoryName = expense.Category.Name
        };
    }
}