using ExpenseTracker.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly ExpenseTrackerDbContext _context;

    public DashboardController(ExpenseTrackerDbContext context)
    {
        _context = context;
    }

    [HttpGet("monthly")]
    public async Task<IActionResult> GetMonthlyDashboard(
        [FromQuery] int month,
        [FromQuery] int year)
    {
        var expenses = _context.Expenses
            .AsNoTracking()
            .Where(e => e.Date.Month == month && e.Date.Year == year);

        var totalSpent = await expenses.SumAsync(e => e.Amount);

        var totalByCategory = await expenses
            .GroupBy(e => e.Category.Name)
            .Select(g => new
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount)
            })
            .OrderByDescending(x => x.Total)
            .ToListAsync();

        var latestExpenses = await expenses
            .OrderByDescending(e => e.Date)
            .Take(5)
            .Select(e => new
            {
                e.Id,
                e.Description,
                e.Amount,
                e.Date,
                CategoryName = e.Category.Name
            })
            .ToListAsync();

        return Ok(new
        {
            Month = month,
            Year = year,
            TotalSpent = totalSpent,
            TotalByCategory = totalByCategory,
            LatestExpenses = latestExpenses
        });
    }
}