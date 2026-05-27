using ExpenseTracker.Application.Dtos;
using ExpenseTracker.Domain.Entities;

namespace ExpenseTracker.Application.Interfaces.Persistence;

public interface IExpenseRepository
{
    public Task<int> CountAsync(int? month, int? year);

    public Task<IEnumerable<Expense>> GetPagedAsync(
        int? month,
        int? year,
        int pageNumber,
        int pageSize);

    public Task<Expense?> GetByIdAsync(Guid id);

    public Task AddAsync(Expense expense);

    public Task<decimal> GetTotalSpentAsync(int month, int year);
    
    Task<IEnumerable<CategoryTotalDto>> GetTotalByCategoryAsync(int month, int year);

    public Task<IEnumerable<Expense>> GetLatestAsync(int month, int year, int count);
}