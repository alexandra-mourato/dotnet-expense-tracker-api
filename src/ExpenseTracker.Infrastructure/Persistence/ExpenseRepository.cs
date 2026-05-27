using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Application.Interfaces.Persistence;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Infrastructure.Persistence;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ExpenseTrackerDbContext _context;

    public ExpenseRepository(ExpenseTrackerDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountAsync(int? month, int? year)
    {
        var query = _context.Expenses
            .AsNoTracking()
            .AsQueryable();

        if (month.HasValue)
        {
            query = query.Where(e => e.Date.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(e => e.Date.Year == year.Value);
        }

        return await query.CountAsync();
    }

    public async Task<IEnumerable<Expense>> GetPagedAsync(
        int? month,
        int? year,
        int pageNumber,
        int pageSize)
    {
        var query = _context.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .AsQueryable();

        if (month.HasValue)
        {
            query = query.Where(e => e.Date.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(e => e.Date.Year == year.Value);
        }

        return await query
            .OrderByDescending(e => e.Date)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdAsync(Guid id)
    {
        return await _context.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Expense expense)
    {
        _context.Expenses.Add(expense);

        await _context.SaveChangesAsync();
    }
    
    public async Task<decimal> GetTotalSpentAsync(int month, int year)
    {
        return await _context.Expenses
            .AsNoTracking()
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .SumAsync(e => e.Amount);
    }

    public async Task<IEnumerable<CategoryTotalDto>> GetTotalByCategoryAsync(
        int month,
        int year)
    {
        return await _context.Expenses
            .AsNoTracking()
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .GroupBy(e => e.Category.Name)
            .Select(g => new CategoryTotalDto
            {
                Category = g.Key,
                Total = g.Sum(e => e.Amount)
            })
            .OrderByDescending(x => x.Total)
            .ToListAsync();
    }

    public async Task<IEnumerable<Expense>> GetLatestAsync(
        int month,
        int year,
        int count)
    {
        return await _context.Expenses
            .AsNoTracking()
            .Include(e => e.Category)
            .Where(e => e.Date.Month == month && e.Date.Year == year)
            .OrderByDescending(e => e.Date)
            .Take(count)
            .ToListAsync();
    }
}