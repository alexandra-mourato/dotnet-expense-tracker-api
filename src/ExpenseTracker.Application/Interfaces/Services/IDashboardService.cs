using ExpenseTracker.Application.Dtos.Output;

namespace ExpenseTracker.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardOutputDto> GetMonthlyDashboardAsync(int month, int year);
}